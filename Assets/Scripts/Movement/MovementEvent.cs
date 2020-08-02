using System;
using UnityEngine;

[Serializable]
public class MovementEvent : Event
{
    public Vector3 Position;

    public MovementEvent(int tick, string receiverID, Vector3 position) : base(tick, receiverID)
    {
        Position = position;
    }
}
