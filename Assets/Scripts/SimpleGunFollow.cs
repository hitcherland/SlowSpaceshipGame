using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGunFollow : MonoBehaviour
{
    public Quaternion initialRotation;
    public float maxAngle = 90;

    public void Start()
    {
        initialRotation = transform.rotation;
    }
    public void Update()
    {
        if (Camera.current == null)
            return;
 
        Quaternion newRotation = Camera.current.transform.rotation * Quaternion.FromToRotation(Vector3.up, Vector3.forward);
        transform.rotation = newRotation;      
    }
}
