using System.Collections.Generic;
using UnityEngine;

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
    public GameObject model;
    public int totalBaseHealth;
    protected int currentBaseHealth;
    public float enginePower;
    public float engineRotationSpeed;
    public Dictionary<string, int> moduleKeyBindings;
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
        Instantiate(model, transform);
    }
}