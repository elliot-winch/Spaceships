using System;
using UnityEngine;
using Winch;

//TODO: rotation


[Serializable]
public class ShipMovementEvent : Event
{
    public Path Path { get; private set; }
    
    public ShipMovementEvent(int tick, string receiverID, Path path) : base(tick, receiverID)
    {
        Path = path;
    }
}

public class ShipMovement : IEventReceiver, IUpdateable
{
    public string ID => "Temp";

    public PhysicsBody PhysicsBody = new PhysicsBody();

    private IRoute m_Route;

    public void ReceiveEvent(Event evt)
    {
        ShipMovementEvent mvtEvt = evt as ShipMovementEvent;

        m_Route = new SimpleAccelerationRoute(PhysicsBody.Velocity.Value);
        m_Route.CreateRoute(mvtEvt.Path);
    }

    public void Update(UpdateInfo updateInfo)
    {
        PhysicsBody.Position.Value = m_Route.SamplePosition(updateInfo.Time);
    }
}
