using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientJoinEvent
{
    public int timestamp;
    public string modelAssetPath;
    public Transform transform;
    public int team;
    public Ship ship;
    public bool youOwnThis = false;
}
