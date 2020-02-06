using System;
using System.IO;
using UnityEngine;
using GameProtobufs;

public class State : BaseMessage<StateMessage>
{
    override public void fromProtobuf(StateMessage msg)
    {
        timestamp = msg.Timestamp;
    }

    override public StateMessage toProtobuf()
    {
        DateTime now = System.DateTime.Now;
        int timestamp = (int)now.ToFileTimeUtc();


        StateMessage output = new StateMessage
        {
            Timestamp = timestamp,
            PathToGameObject = getPathToGameObject(),
            Transform = transform.ToTransformMessage()
        };


        Ship ship = gameObject.GetComponent<Ship>();
        if(ship != null)
        {
            output.ShipMessage = ship.toProtobuf();
        }

        foreach (State state in GetComponentsInChildren<State>())
        {
            if(state != this)
                output.StateMessages.Add(state.toProtobuf());
        }
        return output;
    }

    override public bool Equal(MessageInterface<StateMessage> other)
    {
        if (getPathToGameObject() != other.getPathToGameObject())
            return false;

        return true;
    }

    override public bool Equal(StateMessage other)
    {
        if (getPathToGameObject() != other.PathToGameObject)
            return false;

        return true;
    }

    public void Apply(StateMessage msg)
    {
        //if (msg.Timestamp < timestamp)
        //{
        //    return;
        //}

        if (msg.Transform != null)
        {
            transform.FromTransformMessage(msg.Transform);
        }

        if (msg.ShipMessage != null)
        {
            Ship ship = GetComponent<Ship>();
            if (ship == null)
            {
                ship = Ship.fromProtobuf(gameObject, msg.ShipMessage);
            } else
            {
                ship.fromProtobuf(msg.ShipMessage);
            }
        }

        //Debug.Log("[" + msg.PathToGameObject + "] stateMessages: " + msg.StateMessages);
        foreach (StateMessage sm in msg.StateMessages)
        {
            string path = sm.PathToGameObject;
            string name = Path.GetFileName(path);
            Transform childTransform = transform.Find(name);
            GameObject child;
            if (childTransform == null)
            {
                child = new GameObject(name);
                child.transform.parent = transform;
            }
            else
            {
                child = childTransform.gameObject;
            }

            State state = child.GetComponent<State>();
            if (state == null)
                state = child.AddComponent<State>();

            state.Apply(sm);
        }
    }
}
