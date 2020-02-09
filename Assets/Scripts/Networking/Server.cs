using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using GameProtobufs;
using STUN;

public static class NetworkSettings
{
    public const int defaultPort = 34266;
    public const int defaultBufferSize = 1024;
    public const int defaultTimeoutDelay = 60;
    public const int defaultPingGapDuration = 1;
    public enum ServiceType
    {
        PING,
        PONG,
        CONNECT,
        DISCONNECT,
        LOBBY,
        GAMEBEGIN,
        GAMEUPDATE,
        GAMEEND,
    }
}

public class Server : MonoBehaviour
{
    public int port = NetworkSettings.defaultPort;
    public int bufferSize = NetworkSettings.defaultBufferSize;
    public float timeoutDelay = NetworkSettings.defaultTimeoutDelay;
    public int pingGapDuration = NetworkSettings.defaultPingGapDuration;

    private Socket socket;
    private List<EndPoint> clients = new List<EndPoint>();
    private Dictionary<EndPoint, DateTime> lastContactWithClient = new Dictionary<EndPoint, DateTime>();
    private Dictionary<EndPoint, DateTime> lastPingToClient = new Dictionary<EndPoint, DateTime>();
    private Dictionary<EndPoint, Guid> clientGuids = new Dictionary<EndPoint, Guid>();
    private GameObjectManager manager;
    private delegate void ToDoFunc();
    private Queue<ToDoFunc> toDos = new Queue<ToDoFunc>();

    public IPEndPoint publicEndPoint;

    ~Server()
    {
        Disconnect();
        socket.Close();
    }

    public void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        manager = GetComponent<GameObjectManager>();
        Connect();
    }

    public void Update()
    {
        if (socket != null && socket.IsBound)
        {
            Receive();
            while (toDos.Count > 0)
            {
                toDos.Dequeue()?.Invoke();
            }
            broadcastUPDATE();
            foreach (EndPoint endPoint in lastContactWithClient.Keys)
            {
                DateTime lastContact = lastContactWithClient[endPoint];
    
                if (lastContact.AddSeconds(timeoutDelay) < DateTime.Now)
                {
                    TIMEOUT(endPoint);
                }
            }
        }

    }

    public void Connect()
    {
        clients.Clear();
        lastContactWithClient.Clear();
        clientGuids.Clear();
        toDos.Clear();

        IPEndPoint stunEndPoint = new IPEndPoint(IPAddress.Any, 0);
        foreach(IPAddress ip in Dns.GetHostAddresses("stun3.l.google.com"))
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                stunEndPoint = new IPEndPoint(ip, 19302);
        }

        if (stunEndPoint.Address == IPAddress.Any)
            throw new Exception("STUN failed");

        socket.Bind(new IPEndPoint(IPAddress.Any, port));
        Debug.Log("[Server][" + DateTime.Now + "] START " + socket.LocalEndPoint);

        STUNQueryResult queryResult = STUNClient.Query(socket, stunEndPoint, STUNQueryType.PublicIP);
        publicEndPoint = queryResult.PublicEndPoint;

        Debug.Log("[Server][" + DateTime.Now + "] STUN " + publicEndPoint + " via STUN server " + stunEndPoint);
    }

    public void Reconnect()
    {
        socket.Disconnect(true);
        socket.Bind(new IPEndPoint(IPAddress.Any, port));
        clients = new List<EndPoint>();
    }

    public void Disconnect()
    {
        socket.Shutdown(SocketShutdown.Both);
    }

    public void Receive()
    {
        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.SetBuffer(new byte[bufferSize], 0, bufferSize);
        args.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        args.Completed += OnReceive;
        socket.ReceiveFromAsync(args);
    }

    private void OnReceive(object o, SocketAsyncEventArgs args)
    {
        try
        {
            if (args.BytesTransferred == 0)
            {
                return;
            }

            if (args.SocketError != SocketError.Success)
            {
                Debug.Log("[Server][" + DateTime.Now + "] SocketError " + args.SocketError);
                return;
            }

            EndPoint endPoint = args.RemoteEndPoint;
            NetworkSettings.ServiceType serviceType = (NetworkSettings.ServiceType)args.Buffer[0];
            toDos.Enqueue(() => { lastContactWithClient[endPoint] = DateTime.Now; });
            byte[] buffer = new byte[args.BytesTransferred - 1];
            Array.Copy(args.Buffer, 1, buffer, 0, args.BytesTransferred - 1);

            switch (serviceType)
            {
                case NetworkSettings.ServiceType.PING:
                    OnPing(endPoint);
                    break;
                case NetworkSettings.ServiceType.PONG:
                    break;
                case NetworkSettings.ServiceType.CONNECT:
                    OnConnect(endPoint);
                    break;
                case NetworkSettings.ServiceType.GAMEUPDATE:
                    OnGameUpdate(endPoint, buffer);
                    break;
                default:
                    Debug.Log("[Server][" + DateTime.Now + "] BadServiceType " + serviceType + "=" + args.Buffer[0]);
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.Log("[Server][" + DateTime.Now + "] Exception " + e);
        }
    }

    public void OnPing(EndPoint endPoint)
    {
        Debug.Log("[Server][" + DateTime.Now + "] Receive PING " + endPoint);
        PONG(endPoint);
    }

    public void OnGameUpdate(EndPoint endPoint, byte[] buffer)
    {
        Debug.Log("[Server][" + DateTime.Now + "] Receive Update " + endPoint);
        StateMessage msg = StateMessage.Parser.ParseFrom(buffer);
        toDos.Enqueue(() => { manager.Apply(msg); });
    }

    public void OnConnect(EndPoint endPoint)
    {
        Debug.Log("[Server][" + DateTime.Now + "] Receive CONNECT " + endPoint);
        if (!clients.Contains(endPoint))
        {
            clients.Add(endPoint);
            clientGuids[endPoint] = Guid.NewGuid();
            lastPingToClient[endPoint] = DateTime.Now;


            //TODO: move this somewhere else, it shouldn't happen here
            toDos.Enqueue(() => {
                GameObject ship = manager.CreateCapitalShip(Guid.NewGuid().ToString(), "CapitalShipOne");
                ship.GetComponent<Ship>().ownerGuid = clientGuids[endPoint];
            });
        }

        CONNECT(endPoint, clientGuids[endPoint]);
    }

    public void SENDTO(EndPoint endPoint, NetworkSettings.ServiceType serviceType, byte[] buffer = null)
    {
        if (buffer == null)
        {
            socket.SendTo(new byte[] { (byte)serviceType }, endPoint);
            return;
        }

        byte[] outBuffer = new byte[1 + buffer.Length];
        outBuffer[0] = (byte)serviceType;
        buffer.CopyTo(outBuffer, 1);
        socket.SendTo(outBuffer, endPoint);
    }

    public void TIMEOUT(EndPoint endPoint)
    {
        PING(endPoint);
    }

    public void PING(EndPoint endPoint)
    {
        if (lastPingToClient[endPoint].AddSeconds(pingGapDuration) > DateTime.Now)
        {
            return;
        }
        SENDTO(endPoint, NetworkSettings.ServiceType.PING);
        lastPingToClient[endPoint] = DateTime.Now;
    }

    public void PONG(EndPoint endPoint)
    {
        SENDTO(endPoint, NetworkSettings.ServiceType.PONG);
    }

    public void CONNECT(EndPoint endPoint, Guid guid)
    {
        byte[] guidBytes = guid.ToByteArray();
        SENDTO(endPoint, NetworkSettings.ServiceType.CONNECT, guidBytes);
    }

    public void LOBBY(EndPoint endPoint)
    {
        SENDTO(endPoint, NetworkSettings.ServiceType.LOBBY);
    }

    public void broadcastUPDATE()
    {
        StateMessage message = manager.Collect();
        byte[] buffer = message.ToByteArray();
        foreach (EndPoint endPoint in clients)
        {
            SENDTO(endPoint, NetworkSettings.ServiceType.GAMEUPDATE, buffer);
        }
    }
}