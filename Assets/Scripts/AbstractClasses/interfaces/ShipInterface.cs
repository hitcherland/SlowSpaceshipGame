using System.Collections.Generic;
using UnityEngine;
using GameProtobufs;
using System;

//Todo: implement all the required classes to fully define this interface & base class

public interface ShipInterface
{
    int calculateCurrentHealth();
    void activateModuleMountPoint(int moduleMountPointIndex);
    //void receiveProjectileHit(ProjectileHitEvent ev);
    void die();
    void rotateTowardsPoint(Quaternion rotation, float rotationSpeed);
    void moveInDirection(Vector3 direction, float power);
}

public class Ship : MonoBehaviour, ShipInterface
{
    //public ShipState state;
    public string modelPath;
    public int totalBaseHealth;
    protected int currentBaseHealth;
    public float enginePower;
    public float engineRotationSpeed;
    public Dictionary<string, int> moduleKeyBindings;
    public int team;
    public ShipType type = ShipType.Capital;
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
        UpdateModel(modelPath, true);
    }

    public void UpdateModel(string newModelPath, bool force=false)
    {
        if (!force && modelPath == newModelPath)
            return;

        GameObject prefab = (GameObject)Resources.Load("Prefabs/Ships/" + newModelPath);
        if(prefab == null)
        {
            return;
        }

        modelPath = newModelPath;
        Transform child = transform.Find("model");
        if(child != null)
        {
            Destroy(child.gameObject);
        }

        if (prefab != null)
        {
            GameObject model = Instantiate(prefab, transform);
            model.transform.name = "model";
        }
    }

    public ShipStateMessage toProtobuf()
    {
        return new ShipStateMessage
        {
            Type = type,
            ShipModelName = modelPath,
            //TotalPossibleHealth
            EnginePower = enginePower,
            EngineRotationSpeed = engineRotationSpeed,
            Team = team
        };
    }

    public void fromProtobuf(ShipStateMessage msg)
    {
        enginePower = msg.EnginePower;
        engineRotationSpeed = msg.EngineRotationSpeed;
        team = msg.Team;
    }

    public static Ship fromProtobuf(GameObject owner, ShipStateMessage msg)
    {
        if(msg.Type == ShipType.Capital)
        {
            CapitalShip capital = owner.AddComponent<CapitalShip>();
            capital.fromProtobuf(msg);
            capital.UpdateModel(msg.ShipModelName);
            return capital;
        }
        throw new Exception("unhandled ship type");
    }
}