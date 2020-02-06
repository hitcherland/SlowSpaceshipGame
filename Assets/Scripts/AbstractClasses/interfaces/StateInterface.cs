using System;
using UnityEngine;

public interface MessageInterface<T>
{
    void fromProtobuf(T message);
    T toProtobuf();
    string getPathToGameObject();
    bool Equal(MessageInterface<T> other);
    bool Equal(T other);
}

public class BaseMessage<T> : MonoBehaviour, MessageInterface<T>
{
    public string owner;
    public int timestamp = -1;

    public virtual void fromProtobuf(T msg) { throw new NotImplementedException(); }
    public virtual T toProtobuf() { throw new NotImplementedException(); }
    public string getPathToGameObject() {
        string path = "/" + gameObject.name;
        GameObject obj = gameObject;

        if (obj.transform.parent == null)
            return "/";

        while (obj.transform.parent.parent != null)
        {
            obj = transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
    public virtual bool Equal(MessageInterface<T> other) { throw new NotImplementedException(); }
    public virtual bool Equal(T other) { throw new NotImplementedException(); }
}