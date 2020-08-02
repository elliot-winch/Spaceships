using UnityEngine;

public class ShipMovement : IEventReceiver
{
    public Parameter<Vector3> Position { get; private set; } = new Parameter<Vector3>();

    public string ID => "ShipMovementF";

    public void ReceiveEvent(Event evt)
    { 
        Position.Value = (evt as MovementEvent).Position;
    }
}
