using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject goal;
    public Vector3 relativePoint;
    public Quaternion relativeRotation;
    public float lerpSpeed = 0.1f;
    public Quaternion angleBetween;
    public Quaternion goalRotation;
    public Vector3 goalPosition;
    public Quaternion mouseRot;

    // Start is called before the first frame update
    void Start()
    {
        relativePoint = goal.transform.InverseTransformPoint(transform.position);
        relativeRotation = Quaternion.Inverse(goal.transform.rotation) * transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float getMousePosx = Input.mousePosition.x * 2 * 180 / Screen.width - 180;
        float getMousePosy = Input.mousePosition.y * 2 * 90 / Screen.height - 90;
        //mouseRot = Quaternion.AngleAxis(-getMousePosy, Vector3.right) * Quaternion.AngleAxis(getMousePosx, Vector3.up);
        
        mouseRot = Quaternion.Euler(-getMousePosy, getMousePosx, 0); // * Quaternion.Euler(0, getMousePosx, 0);
        goalPosition = goal.transform.TransformPoint(relativePoint);
        Vector3 goalPos = goal.transform.position;
        Vector3 goalRotated = mouseRot * (goalPosition - goalPos) + goalPos;

        transform.position = goalRotated;
        //transform.position = Vector3.RotateTowards(transform.position - goalPos, goalRotated - goalPos, 0.01f * Mathf.PI, 0) + goalPos;
        //transform.position = Vector3.Slerp(transform.position - mid, goalPosition - mid, lerpSpeed * Time.deltaTime) + mid;

        transform.LookAt(goal.transform, goal.transform.up);

        //goalRotation = goal.transform.rotation * relativeRotation; // * mouseRot;
        //transform.rotation = goalRotation;
        // transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, lerpSpeed * Time.deltaTime);

        Debug.Log(getMousePosx + ", " + getMousePosy);
    }
}