using System;

//TODO: rotation

namespace Winch.Spaceships
{
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

        public RoutePlanner RoutePlanner { get; private set; }

        public ShipMovement(RoutePlanner route)
        {
            RoutePlanner = route;
        }

        public void ReceiveEvent(Event evt)
        {
            ShipMovementEvent mvtEvt = evt as ShipMovementEvent;

            RoutePlanner.CreateRoute(mvtEvt.Path);
        }

        public void Update(UpdateInfo updateInfo)
        {
            PhysicsBody.SetFromSpaceTimePoint(RoutePlanner.SamplePosition(updateInfo.Time));
        }
    }
}
