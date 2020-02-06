using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using System;
using System.IO;
using System.Text;
using GameProtobufs.Services;
using System.Net;
using System.Net.Sockets;
using GameProtobufs;

enum ConnectionStatus {
    UNCONNECTED,
    CONNECTED
}

public class StateHandler : MonoBehaviour
{
    private const int BUFFER_SIZE = 1024 ^ 2;
    private byte[] buffer = new byte[BUFFER_SIZE];

    private Client client;
    private Service service;
    private State state;

    private Socket socket;
    private IPEndPoint endpoint;

    private StateMessage waitingState;
    
    public string remoteAddress = "localhost";
    public int remotePort = 16432;

    public string localAddress = "";
    public int localPort = -1;

    public bool IsClient = true;
    public bool IsSetup = false;

    // Start is called before the first frame update
    void Start()
    {
        state = GetComponent<State>();
        if (IsSetup)
            Setup();
    }

    public void Setup()
    {

        if (IsClient)
        {
            client = new Client
            {
                localHost = localAddress,
                remoteHost = remoteAddress,
                localPort = localPort,
                remotePort = remotePort
            };
            client.Connect();
            client.UpdateResponse += new EventHandler<UpdateResponse>(OnUpdateResponse);
        }
        else
        {
            service = new Service
            {
                localHost = localAddress,
                remoteHost = remoteAddress,
                localPort = localPort,
                remotePort = remotePort
            };
            service.Connect();
        }
    }

    void OnUpdateResponse(object s, UpdateResponse resp)
    {
        waitingState = resp.State;
    }

    // Update is called once per frame
    void Update()
    {
        if (client == null && service == null)
            return;

        if(IsClient)
        {
            if (!client.IsJoined) { client.Join(); }

            if (waitingState != null)
            {
                state.Apply(waitingState);
                waitingState = null;
            }
            client.Receive();
        } else
        {
            StateMessage msg = SimplifyStates();
            msg.Transform = null;
            msg.Timestamp = (int)(1000 * Time.time);
            if(msg != null)
            {
                service.Update(msg);
            }
            service.Receive();
        }
    }

    StateMessage SimplifyStates()
    {
        return state.toProtobuf();
    }
}
