using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameObjectManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateCapitalShip(Guid guid)
    {
        GameObject shipResource = (GameObject)Resources.Load("Prefabs/CapitalShip");
        GameObject shipObject = Instantiate(shipResource);
        shipObject.transform.parent = transform;
        shipObject.transform.localPosition = Vector3.zero;
        shipObject.transform.localRotation = Quaternion.identity;
        shipObject.GetComponent<Ship>().ownerGuid = guid;
    }
}
