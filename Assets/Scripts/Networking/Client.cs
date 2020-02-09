using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using GameProtobufs;

public class Client : MonoBehaviour
{
    public string serverAddress = "localhost";
    public int serverPort = NetworkSettings.defaultPort;
    public int bufferSize = NetworkSettings.defaultBufferSize;
    public float timeoutDelay = NetworkSettings.defaultTimeoutDelay;
    public float pingGapDuration = NetworkSettings.defaultPingGapDuration;
    public string guid;

    private Socket socket;
    private DateTime lastContactWithServer;
    private DateTime lastPing;
    private GameObjectManager manager;
    private delegate void ToDoFunc();
    private Queue<ToDoFunc> toDos = new Queue<ToDoFunc>();

    public void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        manager = GetComponent<GameObjectManager>();
    }

    public void Update()
    {
        if(socket != null && socket.Connected)
        {
            Receive();
            UPDATE();
            if (lastContactWithServer.AddSeconds(timeoutDelay) < DateTime.Now)
            {
                TIMEOUT();
            }
            
        }

        while(toDos.Count > 0)
        {
            toDos.Dequeue()();
        }
    }

    public void Connect()
    {
        lastContactWithServer = DateTime.Now;
        lastPing = DateTime.Now;
        socket.Connect(serverAddress, serverPort);
        CONNECT();
    }


    public void Receive()
    {
        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.SetBuffer(new byte[bufferSize], 0, bufferSize);
        args.Completed += OnReceive;
        socket.ReceiveAsync(args);
    }

    private void OnReceive(object o, SocketAsyncEventArgs args)
    {
        if (args.BytesTransferred == 0)
        {
            return;
        }

        try
        {
            if (args.SocketError != SocketError.Success)
            {
                Debug.Log("[Client][" + DateTime.Now + "] SocketError " + args.SocketError);
                return;
            }

            EndPoint endPoint = args.RemoteEndPoint;
            NetworkSettings.ServiceType serviceType = (NetworkSettings.ServiceType)args.Buffer[0];
            byte[] buffer = new byte[args.BytesTransferred - 1];
            Array.Copy(args.Buffer, 1, buffer, 0, args.BytesTransferred - 1);

            lastContactWithServer = DateTime.Now;
            switch (serviceType)
            {
                case NetworkSettings.ServiceType.PING:
                    OnPing();
                    break;
                case NetworkSettings.ServiceType.PONG:
                    break;
                case NetworkSettings.ServiceType.CONNECT:
                    OnConnect(buffer);
                    break;
                case NetworkSettings.ServiceType.LOBBY:
                    OnLobby();
                    break;
                case NetworkSettings.ServiceType.GAMEUPDATE:
                    OnGameUpdate(buffer);
                    break;
                default:
                    Debug.Log("[Server][" + DateTime.Now + "] BadServiceType " + serviceType + "=" + args.Buffer[0]);
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("[Client][" + DateTime.Now + "] Exception " + e);
        }
    }

    public void OnPing()
    {
        Debug.Log("[Client][" + DateTime.Now + "] Receive PING");
        PONG();
    }

    public void OnLobby()
    {
        Debug.Log("[Client][" + DateTime.Now + "] Receive LOBBY");
    }

    public void OnConnect(byte[] buffer)
    {
        guid = new Guid(buffer).ToString();
        Debug.Log("[Client][" + DateTime.Now + "] Receive CONNECT " + guid);

        //toDos.Enqueue(() => { manager.CreateCapitalShip(guid); });
    }

    public void OnGameUpdate(byte[] buffer)
    {
        StateMessage msg = StateMessage.Parser.ParseFrom(buffer);
        toDos.Enqueue(() => { manager.Apply(msg); });
    }

    public void SEND(NetworkSettings.ServiceType serviceType, byte[] buffer = null)
    {
        if (buffer == null)
        {
            socket.SendTo(new byte[] { (byte)serviceType }, socket.RemoteEndPoint);
            return;
        }

        byte[] outBuffer = new byte[1 + buffer.Length];
        outBuffer[0] = (byte)serviceType;
        buffer.CopyTo(outBuffer, 1);
        socket.SendTo(outBuffer, socket.RemoteEndPoint);
    }

    public void UPDATE()
    {
        Debug.Log("[Client][" + DateTime.Now + "] Send UPDATE");
        StateMessage message = manager.Collect();
        byte[] buffer = message.ToByteArray();
        SEND(NetworkSettings.ServiceType.GAMEUPDATE, buffer);
    }

    private void PING()
    {
        if (lastPing.AddSeconds(pingGapDuration) > DateTime.Now)
        {
            return;
        }

        Debug.Log("[Client][" + DateTime.Now + "] Send PING" + socket.RemoteEndPoint);
        SEND(NetworkSettings.ServiceType.PING);
        lastPing = DateTime.Now;
    }

    private void PONG()
    {
        Debug.Log("[Client][" + DateTime.Now + "] Send PONG" + socket.RemoteEndPoint);
        SEND(NetworkSettings.ServiceType.PONG);
    }

    private void CONNECT()
    {
        Debug.Log("[Client][" + DateTime.Now + "] Send CONNECT " + socket.RemoteEndPoint);
        SEND(NetworkSettings.ServiceType.CONNECT);
    }

    private void TIMEOUT()
    {
        PING();
    }
}