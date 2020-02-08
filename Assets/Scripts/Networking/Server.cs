using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using GameProtobufs.Services;
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

    public bool useIpV6 = false;

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
        socket = new Socket(useIpV6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        manager = GetComponent<GameObjectManager>();
        Connect();
    }

    public void Update()
    {
        if (socket != null && socket.IsBound)
        {
            Receive();

            foreach (EndPoint endPoint in lastContactWithClient.Keys)
            {
                DateTime lastContact = lastContactWithClient[endPoint];
    
                if (lastContact.AddSeconds(timeoutDelay) < DateTime.Now)
                {
                    TIMEOUT(endPoint);
                }
            }
        }

        while (toDos.Count > 0)
        {
            toDos.Dequeue()();
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
            if (ip.AddressFamily == (useIpV6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork))
                stunEndPoint = new IPEndPoint(ip, 19302);
        }

        if (stunEndPoint.Address == IPAddress.Any)
            throw new Exception("STUN failed");

        socket.Bind(new IPEndPoint(useIpV6 ? IPAddress.IPv6Any : IPAddress.Any, port));
        Debug.Log("[Server][" + DateTime.Now + "] START " + socket.LocalEndPoint);

        STUNQueryResult queryResult = STUNClient.Query(socket, stunEndPoint, STUNQueryType.PublicIP);
        publicEndPoint = queryResult.PublicEndPoint;

        Debug.Log("[Server][" + DateTime.Now + "] STUN " + publicEndPoint + " via STUN server " + stunEndPoint);
    }

    public void Reconnect()
    {
        socket.Disconnect(true);
        socket.Bind(new IPEndPoint(useIpV6 ? IPAddress.IPv6Any : IPAddress.Any, port));
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
        args.RemoteEndPoint = new IPEndPoint(useIpV6 ? IPAddress.IPv6Any : IPAddress.Any, 0);
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
            lastContactWithClient[endPoint] = DateTime.Now;

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

    public void OnConnect(EndPoint endPoint)
    {
        Debug.Log("[Server][" + DateTime.Now + "] Receive CONNECT " + endPoint);
        if (!clients.Contains(endPoint))
        {
            clients.Add(endPoint);
            clientGuids[endPoint] = Guid.NewGuid();
            lastPingToClient[endPoint] = DateTime.Now;


            //TODO: move this somewhere else, it shouldn't happen here
            toDos.Enqueue(() => { manager.CreateCapitalShip(clientGuids[endPoint]); });
        }

        CONNECT(endPoint, clientGuids[endPoint]);
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
        socket.SendTo(new byte[] { (byte)NetworkSettings.ServiceType.PING }, endPoint);
        lastPingToClient[endPoint] = DateTime.Now;
    }

    public void PONG(EndPoint endPoint)
    {
        socket.SendTo(new byte[] { (byte)NetworkSettings.ServiceType.PONG }, endPoint);
    }

    public void CONNECT(EndPoint endPoint, Guid guid)
    {
        byte[] guidBytes = guid.ToByteArray();
        byte[] buffer = new byte[1 + guidBytes.Length];
        buffer[0] = (byte)NetworkSettings.ServiceType.CONNECT;
        guidBytes.CopyTo(buffer, 1);
        socket.SendTo(buffer, endPoint);
    }

    public void LOBBY(EndPoint endPoint)
    {
        socket.SendTo(new byte[] { (byte)NetworkSettings.ServiceType.LOBBY }, endPoint);
    }
}