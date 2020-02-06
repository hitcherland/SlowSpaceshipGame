using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using GameProtobufs;
using GameProtobufs.Services;
using System;
using System.Net;
using System.Net.Sockets;

public class RequestArgs
{
    public RequestMessage request;
    public EndPoint endpoint;
    public ResponseMessage response = new ResponseMessage();
}

public class JoinRequestArgs
{
    public JoinRequest request;
    public EndPoint endpoint;
    public JoinResponse response = new JoinResponse();
}

public class BaseNetwork
{
    protected Socket socket;
    public string localHost = "127.0.0.1";
    public string remoteHost = "127.0.0.1";
    public int localPort = 8081;
    public int remotePort = 8082;
    public AddressFamily addressFamily = AddressFamily.InterNetwork;
    public bool IsReady { get; private set; }

    protected EndPoint localEndpoint;
    protected EndPoint remoteEndpoint;
    private int buffer_size = 1024;
    private event EventHandler<RequestArgs> Request;
    public event EventHandler<JoinRequestArgs> JoinRequest;
    public event EventHandler<LeaveRequest> LeaveRequest;
    public event EventHandler<UpdateRequest> UpdateRequest;

    private event EventHandler<ResponseMessage> Response;
    public event EventHandler<JoinResponse> JoinResponse;
    public event EventHandler<LeaveResponse> LeaveResponse;
    public event EventHandler<UpdateResponse> UpdateResponse;

    ~BaseNetwork()
    {
        if(socket != null && socket.IsBound)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(true);
        }
    }

    public void Receive(EventHandler<SocketAsyncEventArgs> onComplete)
    {
        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        byte[] buffer = new byte[buffer_size];
        args.SetBuffer(buffer, 0, buffer_size);
        args.RemoteEndPoint = remoteEndpoint;
        args.Completed += onComplete;
        socket.ReceiveFromAsync(args);
    }

    public void Connect()
    {
        socket = new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp);

        IPAddress remoteIP = IPAddress.Parse(remoteHost);
        remoteEndpoint = new IPEndPoint(remoteIP, remotePort);

        if(localHost != "")
        {
            IPAddress localIP = IPAddress.Parse(localHost);
            localEndpoint = new IPEndPoint(localIP, localPort);
            socket.Bind(localEndpoint);
        } else
        {
            socket.Connect(remoteEndpoint);
            Debug.Log(socket.Connected);
            Debug.Log(socket.LocalEndPoint);
            Debug.Log(socket.RemoteEndPoint);
        }

        IsReady = true;
    }

    protected void OnRequest(object s, SocketAsyncEventArgs args)
    {
        if (args.BytesTransferred == 0)
            return;

        Debug.Log(args.BytesTransferred);

        RequestMessage req = RequestMessage.Parser.ParseFrom(args.Buffer, 0, args.BytesTransferred);

        if (req.ServiceType == ServiceTypes.Join)
        {

            JoinRequestArgs jargs = new JoinRequestArgs { request = req.JoinRequest, endpoint = args.RemoteEndPoint };
            JoinRequest?.Invoke(s, jargs);
            ResponseMessage resp = new ResponseMessage
            {
                ServiceType = ServiceTypes.Join,
                JoinResponse = jargs.response
            };
            int x = socket.SendTo(resp.ToByteArray(), args.RemoteEndPoint);
            //Debug.Log("[" + args.RemoteEndPoint + "]response time:" + ((jargs.response.Timestamp - req.JoinRequest.Timestamp) / 10000.0));
        }
    }

    protected void OnResponse(object s, SocketAsyncEventArgs args)
    {
        if (args.BytesTransferred == 0)
            return;
        //Debug.Log("Received Response bytes: " + args.BytesTransferred);
        try
        {
            ResponseMessage resp = ResponseMessage.Parser.ParseFrom(args.Buffer, 0, args.BytesTransferred);
            if (resp.ServiceType == ServiceTypes.Join)
            {
                JoinResponse?.Invoke(s, resp.JoinResponse);
            }
            else if (resp.ServiceType == ServiceTypes.Update)
            {
                UpdateResponse?.Invoke(s, resp.UpdateResponse);

            }
        }
        catch (Exception e)
        {
            Debug.Log("ResponseException: " + e);
        }
    }
}


public class Client : BaseNetwork
{
    public bool IsJoined { get; private set; }
    public Client()
    {
        JoinResponse += new EventHandler<JoinResponse>(OnJoin);
        
    }

    public void Receive()
    {
        Receive(new EventHandler<SocketAsyncEventArgs>(OnResponse));
    }

    public void OnJoin(object s, JoinResponse response)
    {
        Debug.Log("joined!");
        IsJoined = true;
    }

    public void Join()
    {
        RequestMessage req = new RequestMessage
        {
            ServiceType = ServiceTypes.Join,
            JoinRequest = new JoinRequest()
        };
        req.JoinRequest.Timestamp = (int)DateTime.Now.ToFileTimeUtc();
        Debug.Log("sent join");
        socket.SendTo(req.ToByteArray(), remoteEndpoint);
    }
}

public class Service : BaseNetwork
{
    Dictionary<string, EndPoint> clients = new Dictionary<string, EndPoint>();
    public Service()
    {
        JoinRequest += new EventHandler<JoinRequestArgs>(OnJoin);
    }

    public void Receive()
    {
        Receive(new EventHandler<SocketAsyncEventArgs>(OnRequest));
    }

    public void OnJoin(object s, JoinRequestArgs args)
    {   
        EndPoint endpoint = args.endpoint;
        if (endpoint != null && !clients.ContainsKey(endpoint.ToString()))
        {
            clients[endpoint.ToString()] = endpoint;
            Debug.Log("Recording new join:" + endpoint.ToString());
        }
        args.response.Timestamp = (int)DateTime.Now.ToFileTimeUtc();
    }

    public void Update(StateMessage msg)
    {
        ResponseMessage resp = new ResponseMessage
        {
            ServiceType = ServiceTypes.Update,
            UpdateResponse = new UpdateResponse
            {
                State = msg
            }
        };

        byte[] buffer = resp.ToByteArray();

        foreach(EndPoint ep in clients.Values)
        {
            if(ep != null)
                socket.SendTo(buffer, ep);
        }
    }
}