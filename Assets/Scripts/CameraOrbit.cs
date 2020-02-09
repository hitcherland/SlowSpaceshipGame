using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform focus;
    public float sensitivity = 1f;
    public float returnSpeed = 0f;
    public float extraDistanceDueToSpeed = 0.0f;
    public bool invertYAxis = false;
    public float reqZoom = 0;
    public float actualZoom = 0;
    public float zoomFactor = 0.999f;

    private float x = 0, y = 0;
    private Vector3 initialPoint;
    private Quaternion initialRotation;

    /*This should probably be replaced with some generic way
     * of calculating focus speed
     */
    private CapitalShip ship;

    // Start is called before the first frame update
    void Start()
    {
        if (focus != null)
            Initiate();

        reqZoom = actualZoom;

    }

    public void Initiate()
    {
        ship = focus.GetComponent<CapitalShip>();
        initialPoint = focus.InverseTransformPoint(transform.position);
        initialRotation = Quaternion.Inverse(focus.localRotation) * transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (focus == null)
            return;

        // determine whether we want to change the camera's zoom
        if (Input.GetAxis("Mouse ScrollWheel") > 0 ){
            reqZoom += 0.10f;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            reqZoom += -0.10f;
        }

        actualZoom = Mathf.Lerp(actualZoom, reqZoom, zoomFactor * Time.deltaTime);

        // Use incremental mouse rotations
        x += sensitivity * Input.GetAxis("Mouse X");
        y += (invertYAxis ? 1 : -1) * sensitivity * Input.GetAxis("Mouse Y");

        // We can have the camera return to it's default position
        x = Mathf.Lerp(x, 0, returnSpeed * Time.deltaTime);
        y = Mathf.Lerp(y, 0, returnSpeed * Time.deltaTime);

        /* We have the goal position for the camera in the local space of the focus
         * We rotate that point around the origin (i.e. focus) by the mouse rotation
         * and then transform that local space value to a world space value
         */
        Quaternion mouseRotation = Quaternion.Euler(y, x, 0);
        Vector3 rotatedPoint = mouseRotation * initialPoint * (1 + extraDistanceDueToSpeed * (ship.effectivePowLevel) / 100);
        Vector3 transformedPoint = focus.TransformPoint(rotatedPoint);
 
        transform.position = transformedPoint;

        /* transform.LookAt(focus); was causing us gimbal locking issues
         * instead we just do the transformation manually
         */
        transform.rotation = focus.localRotation * initialRotation * mouseRotation;

        // apply requested zoom translation
        transform.Translate(initialPoint * actualZoom);
    }
}
