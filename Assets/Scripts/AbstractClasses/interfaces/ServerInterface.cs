using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ServerInterface
{
    //void receiveRemoteState(StateMessage remoteState);
    //void sendState();
    //void addClient(Client newClient);
    //void removeClient(Client oldClient);
    //void rejoinClient(Client newClient);
    //void handleCheating(StateMessage newState);
    void send_update();
}


public class BaseServer : MonoBehaviour, ServerInterface
{
    //public StateMessage state;
    //Client[] clients;
    //GameServerPreferences preferences;
    //Dictionary<string, Client> objectPathToClient;
    //Dictionary<Client, string[]> clientToObjectPath;
    public void send_update() { throw new NotImplementedException(); }
}
