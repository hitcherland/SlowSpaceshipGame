using System;
using GameProtobufs;

public class State : BaseMessage<StateMessage>
{
    override public void fromProtobuf(StateMessage msg)
    {
        timestamp = DateTime.FromFileTimeUtc((long)msg.Timestamp);
        
    }

    override public StateMessage toProtobuf()
    {
        DateTime now = System.DateTime.Now;
        long timestamp = now.ToFileTimeUtc();

        return new StateMessage
        {
            Timestamp = (int)timestamp,
            PathToGameObject = getPathToGameObject()
        };
    }
}
