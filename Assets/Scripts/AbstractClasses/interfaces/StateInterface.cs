using System;
using UnityEngine;

public interface MessageInterface<T>
{
    void fromProtobuf(T message);
    T toProtobuf();
    string getPathToGameObject();
}

public class BaseMessage<T> : MonoBehaviour, MessageInterface<T>
{
    public string owner;
    public DateTime timestamp;

    public virtual void fromProtobuf(T msg) { throw new NotImplementedException(); }
    public virtual T toProtobuf() { throw new NotImplementedException(); }
    public string getPathToGameObject() {
        string path = "/" + gameObject.name;
        GameObject obj = gameObject;
        while(obj.transform.parent.gameObject != null)
        {
            path = "/" + obj.name + path;
        }
        return path;
    }
}