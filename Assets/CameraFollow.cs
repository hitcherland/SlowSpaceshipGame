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

    //static float customSlerp(Vector3 x, Vector3 y, float angle, float t)
    //{

    //}

    // Start is called before the first frame update
    void Start()
    {
        relativePoint = goal.transform.InverseTransformPoint(transform.position);
        relativeRotation = Quaternion.Inverse(goal.transform.rotation) * transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float getMousePosx = Input.mousePosition.x * 2 * 180 / Screen.width;
        float getMousePosy = Input.mousePosition.y * 2 * 180 / Screen.height;
        mouseRot = Quaternion.AngleAxis(getMousePosx - 180, Vector3.up) * Quaternion.AngleAxis(180 - getMousePosy, Vector3.right);
        

        goalPosition = goal.transform.TransformPoint(mouseRot * relativePoint);
        Vector3 mid = goal.transform.position;

        transform.position = Vector3.Slerp(transform.position - mid, goalPosition - mid, lerpSpeed * Time.deltaTime) + mid;

        transform.LookAt(goal.transform);
        // goalRotation = goal.transform.rotation * relativeRotation * mouseRot;
        // transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, lerpSpeed * Time.deltaTime);

        Debug.Log(getMousePosx);
    }
}