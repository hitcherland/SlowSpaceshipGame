using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapitalShip : Ship
{
    // TODO: check for ownership, allow for input if true
    public float pow = 10;
    public bool owned = true;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: moving forwards
        Quaternion x_quart = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * Time.deltaTime * pow, Vector3.up);
        Quaternion y_quart = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * Time.deltaTime * pow, Vector3.right);

        transform.rotation = transform.rotation * x_quart * y_quart;
    }
}
