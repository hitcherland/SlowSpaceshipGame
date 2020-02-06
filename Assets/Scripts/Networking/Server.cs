using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using GameProtobufs;

public class Server : BaseServer
{
    //private State state;
    // private StateHandler stateHander;
    public string hostAddress = "";
    public int port = 8081;
    private EndPoint endpoint;

    public AddressFamily addressType = AddressFamily.InterNetwork;
    private const int BUFFER_SIZE = 1024 ^ 2;
    private Socket socket;
    private byte[] buffer = new Byte[BUFFER_SIZE];

    // Start is called before the first frame update
    void Start()
    {
        //stateHandler = new StateHandler();
        Bind();
    }

    void Update()
    {

        if (socket == null || !socket.IsBound)
            return;

        SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
        receiveArgs.RemoteEndPoint = endpoint;
        receiveArgs.SetBuffer(buffer, 0, BUFFER_SIZE);
        receiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Receive);

        socket.ReceiveFromAsync(receiveArgs);
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
                    endpoint = new IPEndPoint(ipAddress, port);
                    break;
                }
                catch { }
            }
        }
        if(socket != null)
        {
            Debug.Log("SERVER: binding to " + endpoint);
            socket.Bind(endpoint);
        }
    }

    private void Receive(object sender, SocketAsyncEventArgs asyncArgs)
    {
        if (asyncArgs.BytesTransferred > 0 && asyncArgs.SocketError == SocketError.Success)
        {
            //Service req = Service.Parser.ParseFrom(asyncArgs.Buffer, 0, asyncArgs.BytesTransferred);
            return;
            /*
            if(req.Join != null)
            {
                
                Socket s = (Socket)sender;
                Debug.Log("joining");

                //s.Send(.ToByteArray());
                Debug.Log("joined");
            }
            */
        }
    }

    private void OnDestroy()
    {
        if (socket != null && socket.IsBound)
        {
            socket.Close();
        }
    }
}