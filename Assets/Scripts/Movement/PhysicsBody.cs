using UnityEngine;
using Winch;

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
}
