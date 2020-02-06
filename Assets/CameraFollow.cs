using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject goal;
    public Vector3 relativePoint;
    public Quaternion relativeRotation;
    public float lerpSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        relativePoint = goal.transform.InverseTransformPoint(transform.position);
        relativeRotation = Quaternion.Inverse(goal.transform.rotation) * transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 goalPosition = goal.transform.TransformPoint(relativePoint);
        transform.position = Vector3.Lerp(transform.position, goalPosition, lerpSpeed * Time.deltaTime);
        Quaternion goalRotation = goal.transform.rotation * relativeRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, lerpSpeed * Time.deltaTime);
    }
}