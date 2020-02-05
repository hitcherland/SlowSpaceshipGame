using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShip : Ship
{
    // TODO: check for ownership, allow for input if true
    public bool owned = true;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetPosition;

    float pitch;
    float roll;
    float yaw;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // get axes
        roll = Input.GetAxis("Horizontal");
        pitch = -Input.GetAxis("Vertical");

        // rotate
        transform.Rotate(Vector3.back * roll * 100f * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.right * pitch * 100f * Time.deltaTime, Space.Self);

        // alter speed with space (for now)
        if (Input.GetKey("space")) transform.Translate(Vector3.forward * enginePower * Time.deltaTime);
    }
}
