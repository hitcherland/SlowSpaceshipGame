using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Google.Protobuf;
using GameProtobufs;


public interface Watchable
{
    Guid GetGuid();
    Type GetProtobufType();
    bool HasChanged();
    IMessage ToProtobuf();
    void FromProtobuf(IMessage protobuf);
}

public class GameObjectManager : MonoBehaviour
{
    private Client client;
    // Start is called before the first frame update
    void Start()
    {
        client = transform.GetComponent<Client>();
    }

    public GameObject CreateCapitalShip(string name, string model)
    {
        GameObject shipPrefab = (GameObject)Resources.Load("Prefabs/Ships/Capital/" + model);
        GameObject shipObject = Instantiate(shipPrefab);
        shipObject.transform.name = name;
        shipObject.transform.parent = transform;
        shipObject.transform.localPosition = Vector3.zero;
        shipObject.transform.localRotation = Quaternion.identity;
        shipObject.GetComponent<Ship>().model = model;

        return shipObject;
    }


    public GameObject CreateCamera(GameObject shipObject, string model)
    {
        GameObject cameraPrefab = (GameObject)Resources.Load("Prefabs/Cameras/Capital/" + model + " Camera");
        GameObject cameraObject = Instantiate(cameraPrefab);
        cameraObject.transform.name = "Main Camera";
        CameraOrbit orbit = cameraObject.GetComponentInChildren<CameraOrbit>();
        orbit.focus = shipObject.transform;
        orbit.Initiate();
        
        return cameraObject;
    }

    public StateMessage Collect(bool ignoreUnchanged=true)
    {
        StateMessage msg = new StateMessage();

        Watchable[] watchables = GetComponentsInChildren<Watchable>();
        foreach(Watchable watchable in watchables)
        {
            if (client != null && client.guid.CompareTo(watchable.GetGuid()) != 0)
                continue;

            if (!ignoreUnchanged || watchable.HasChanged())
            {
                IMessage protobuf = watchable.ToProtobuf();
                Type type = watchable.GetProtobufType();
                if(type == typeof(ShipMessage))
                {
                    msg.Ships.Add((ShipMessage)protobuf);
                }
            }
        }
        return msg;
    }

    public void Apply(StateMessage msg)
    {
        foreach(ShipMessage shipMessage in msg.Ships)
        {
            Transform childTransform = transform.Find(shipMessage.Id);
            if(childTransform == null)
            {
                GameObject shipObject = CreateCapitalShip(shipMessage.Id, shipMessage.Model);
                childTransform = shipObject.transform;
                CapitalShip ship = childTransform.GetComponent<CapitalShip>();
                ship.FromProtobuf(shipMessage);

                if (client != null && client.guid.CompareTo(ship.ownerGuid) == 0)
                {
                    GameObject camera = CreateCamera(shipObject, shipMessage.Model);
                }
            } else
            {
                childTransform.GetComponent<CapitalShip>().FromProtobuf(shipMessage);
            }
            
            childTransform.hasChanged = false;
        }
    }
}
