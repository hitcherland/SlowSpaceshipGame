using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameProtobufs;

public static class Conversions
{
    public static Vector3 ToVector3(this Vector3Message msg)
    {
        return new Vector3(msg.X, msg.Y, msg.Z);
    }

    public static Vector3Message ToVector3Message(this Vector3 vec)
    {
        return new Vector3Message { X = vec.x, Y = vec.y, Z = vec.z };
    }

    public static Quaternion ToQuaternion(this QuaternionMessage msg)
    {
        return new Quaternion(msg.X, msg.Y, msg.Z, msg.W);
    }

    public static QuaternionMessage ToQuaternionMessage(this Quaternion quat)
    {
        return new QuaternionMessage { X = quat.x, Y = quat.y, Z = quat.z, W = quat.w };
    }

    public static void FromTransformMessage(this Transform transform, TransformMessage msg)
    {
        transform.localPosition = msg.Position.ToVector3();
        transform.localRotation = msg.Quaternion.ToQuaternion();
        //transform.SetPositionAndRotation(msg.Position.ToVector3(), msg.Quaternion.ToQuaternion());
    }

    public static TransformMessage ToTransformMessage(this Transform transform)
    {
        return new TransformMessage
        {
            Position = transform.localPosition.ToVector3Message(),
            Quaternion = transform.localRotation.ToQuaternionMessage()
        };
    }
}
