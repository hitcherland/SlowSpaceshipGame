using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using GameProtobufs;
using System;

public class Ship : MonoBehaviour, Watchable
{
    //public ShipState state;
    public string model;
    public int totalBaseHealth;
    protected int currentBaseHealth;
    public float enginePower;
    public float engineRotationSpeed;
    public Dictionary<string, int> moduleKeyBindings;
    public int team;
    public ShipType type = ShipType.Capital;
    public string ownerGuid;
    //ModuleMountPoint[] moduleMountPoints;

    public virtual int calculateCurrentHealth() { throw new NotImplementedException(); }
    public virtual void activateModuleMountPoint(int moduleMountPointIndex) { throw new NotImplementedException(); }
    //private virtual void receiveProjectileHit(ProjectileHitEvent ev) { throw new NotImplementedException(); }
    public virtual void die() { throw new NotImplementedException(); }
    public virtual void rotateTowardsPoint(Quaternion rotation, float rotationSpeed) { throw new NotImplementedException(); }
    public virtual void moveInDirection(Vector3 direction, float power) { throw new NotImplementedException(); }

    /*We should be very strict about defining functions here, only put in what every instance MUST do
     * In this case, we demand that we instantiate the ship model using prefabs (https://docs.unity3d.com/2018.4/Documentation/Manual/Prefabs.html).
     */

    protected void Start()
    {
        UpdateModel(model, true);
    }

    public void UpdateModel(string newModelPath, bool force = false)
    {
        if (!force && model == newModelPath)
            return;

        GameObject prefab = (GameObject)Resources.Load("Prefabs/Ships/" + newModelPath);
        if (prefab == null)
        {
            return;
        }

        model = newModelPath;
        Transform child = transform.Find("model");
        if (child != null)
        {
            Destroy(child.gameObject);
        }

        if (prefab != null)
        {
            GameObject model = Instantiate(prefab, transform);
            model.transform.name = "model";
        }
    }

    public Type GetProtobufType()
    {
        return typeof(ShipMessage);
    }

    public string GetGuid()
    {
        return ownerGuid;
    }

    public bool HasChanged()
    {
        if(transform.hasChanged)
        {
            transform.hasChanged = false;
            return true;
        }
        return false;
    }

    public void FromProtobuf(IMessage message)
    {
        ShipMessage msg = (ShipMessage)message;
        if(msg.Transform != null)
        {
            transform.FromTransformMessage(msg.Transform);
        }

        if(msg.OwnerGuid != null)
        {
            ownerGuid = msg.OwnerGuid;
        }

        if(msg.Model != null && msg.Model != model)
        {
            UpdateModel(msg.Model);
        }
    }

    public IMessage ToProtobuf()
    {
        return new ShipMessage
        {
            Transform = transform.ToTransformMessage(),
            OwnerGuid = ownerGuid,
            Id = transform.name,
            Model = model,
        };
    }
}
