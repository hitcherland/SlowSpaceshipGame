using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CapitalShip : Ship
{
    // TODO: check for ownership, allow for input if true
    public bool owned = true;
    public float powLevel = 0;
    public float effectivePowLevel = 0;
    public float accelFactor = 0.5f;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetPosition;
    private Client client;

    float pitch;
    float roll;
    float yaw;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        effectivePowLevel = powLevel;
        client = transform.parent.GetComponent<Client>();
    }

    // Update is called once per frame
    void Update()
    {
        if (client == null || client.guid != ownerGuid)
            return;
        // get axes
        roll = Input.GetAxis("Horizontal");
        pitch = -Input.GetAxis("Vertical");

        // pitch and roll
        transform.Rotate(Vector3.back * roll * engineRotationSpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.right * pitch * engineRotationSpeed * Time.deltaTime, Space.Self);

        // alter speed with space (for now)
        if (Input.GetButtonDown("Thrust"))
        {
            {
                powLevel += 25 * Input.GetAxis("Thrust");
                powLevel = Mathf.Clamp(powLevel, 0, 100);
            }
        }

        effectivePowLevel = Mathf.Lerp(effectivePowLevel, powLevel, accelFactor * Time.deltaTime);

        transform.Translate(Vector3.forward * enginePower * Time.deltaTime * (effectivePowLevel / 100));

        if (Math.Abs(Input.GetAxis("Jump")) > 0.001)
        {
            transform.Translate(Vector3.up * (enginePower/10) * Time.deltaTime);
        }
    }
}
