using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpin : MonoBehaviour
{
    public float speed = 1;
    private Vector3 spin;
    // Start is called before the first frame update
    void Start()
    {
        spin = Random.onUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(spin * speed * Time.deltaTime);
    }
}
