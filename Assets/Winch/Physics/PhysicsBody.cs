using UnityEngine;

namespace Winch
{
    public class PhysicsBody : IUpdateable
    {
        public Parameter<Vector3> Position { get; private set; } = new Parameter<Vector3>();
        public Parameter<Vector3> Velocity { get; private set; } = new Parameter<Vector3>();
        public Parameter<Vector3> Acceleration { get; private set; } = new Parameter<Vector3>();
        public Parameter<float> Mass { get; private set; } = new Parameter<float>();

        public void Update(UpdateInfo info)
        {
            //Physics is non-deterministic when given different time steps. 
            //Instead, we use a base time and perform 
            //simualtion steps to maintain determinism
            for (int i = 0; i < info.TicksPerUpdate; i++)
            {
                Velocity.Value += Acceleration.Value * info.TicksPerUpdate;
                Position.Value += Velocity.Value * info.TicksPerUpdate;
            }
        }

        public void SetFromSpaceTimePoint(SpaceTimePoint point)
        {
            Position.Value = point.Position;
            Velocity.Value = point.Velocity;
            Acceleration.Value = point.Acceleration;
        } 

        public SpaceTimePoint GetSpaceTimePoint()
        {
            return new SpaceTimePoint()
            {
                Position = Position.Value,
                Velocity = Velocity.Value,
                Acceleration = Acceleration.Value
            };
        }
    }
}
