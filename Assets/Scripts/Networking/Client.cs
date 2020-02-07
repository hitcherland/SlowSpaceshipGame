using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using GameProtobufs.Services;

public class Client : MonoBehaviour
{
    public string serverAddress = "localhost";
    public int serverPort = 34268;
    public int bufferSize = NetworkSettings.defaultBufferSize;
    public float timeoutDelay = NetworkSettings.defaultTimeoutDelay;
    public float pingGapDuration = NetworkSettings.defaultPingGapDuration;
    public Guid guid;

    private Socket socket;
    private DateTime lastContactWithServer;
    private DateTime lastPing;
    private GameObjectManager manager;
    private delegate void ToDoFunc();
    private Queue<ToDoFunc> toDos = new Queue<ToDoFunc>();

    public void Start()
    {
        socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
        manager = GetComponent<GameObjectManager>();
    }

    public void Update()
    {
        if(socket != null && socket.Connected)
        {
            Receive();
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

    private void PING()
    {
        if(lastPing.AddSeconds(pingGapDuration) > DateTime.Now)
        {
            return;
        }

        Debug.Log("[Client][" + Time.time + "] Send PING" + socket.RemoteEndPoint);
        socket.Send(new byte[] { (byte)NetworkSettings.ServiceType.PING });
        lastPing = DateTime.Now;
    }

    private void PONG()
    {
        Debug.Log("[Client][" + Time.time + "] Send PONG" + socket.RemoteEndPoint);
        socket.Send(new byte[] { (byte)NetworkSettings.ServiceType.PONG });
    }

    private void CONNECT()
    {
        Debug.Log("[Client][" + Time.time + "] Send CONNECT " + socket.RemoteEndPoint);
        socket.Send(new byte[] { (byte)NetworkSettings.ServiceType.CONNECT });
    }

    private void TIMEOUT()
    {
        PING();
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
        guid = new Guid(buffer);
        Debug.Log("[Client][" + DateTime.Now + "] Receive CONNECT " + guid);

        toDos.Enqueue(() => { manager.CreateCapitalShip(guid); });
    }
}