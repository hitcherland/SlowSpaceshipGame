using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
//using GameProtobufs;

public class Server : BaseServer
{
    //private State state;
    // private StateHandler stateHander;
    public string hostAddress = "";
    public int port = 8081;
    public AddressFamily addressType = AddressFamily.InterNetwork;

    private const int BUFFER_SIZE = 1024 ^ 2;
    private Socket socket;
    private Int32 backlog = 100; // how many backlog messages we want to read
    private EndPoint endpoint;
    private byte[] buffer = new Byte[BUFFER_SIZE];

    // Start is called before the first frame update
    void Start()
    {
        //stateHandler = new StateHandler();
        Bind();
    }

    void Update()
    {
        SocketAsyncEventArgs asyncArgs = new SocketAsyncEventArgs();
        asyncArgs.RemoteEndPoint = endpoint;
        asyncArgs.SetBuffer(buffer, 0, BUFFER_SIZE);
        asyncArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Receive);

        socket.ReceiveFromAsync(asyncArgs);
    }

    void Bind()
    {
        foreach (IPAddress ip in Dns.GetHostAddresses(hostAddress))
        {
            if (ip.AddressFamily == addressType)
            {
                try
                {
                    IPAddress ipAddress;
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = IPAddress.Any;
                    }
                    else if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        ipAddress = IPAddress.IPv6Any;
                    }
                    else
                    {
                        continue;
                    }
                    socket = new Socket(addressType, SocketType.Dgram, ProtocolType.Udp);
                    endpoint = (EndPoint)(new IPEndPoint(ipAddress, port));
                    break;
                }
                catch { }
            }
        }
        socket.Bind(endpoint);
    }

    private void Receive(object sender, SocketAsyncEventArgs asyncArgs)
    {
        if (asyncArgs.BytesTransferred > 0 && asyncArgs.SocketError == SocketError.Success)
        {
            //State state = State.Parser.ParseFrom(asyncArgs.Buffer, 0, asyncArgs.BytesTransferred);
            //StateHandler.updateState(state);
        }
    }
}