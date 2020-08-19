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

        private RoutePlanner m_Route;

        public ShipMovement(RoutePlanner route)
        {
            m_Route = route;
        }

        public void ReceiveEvent(Event evt)
        {
            ShipMovementEvent mvtEvt = evt as ShipMovementEvent;

            m_Route.CreateRoute(mvtEvt.Path);
        }

        public void Update(UpdateInfo updateInfo)
        {
            PhysicsBody.SetFromSpaceTimePoint(m_Route.SamplePosition(updateInfo.Time));
        }
    }
}
