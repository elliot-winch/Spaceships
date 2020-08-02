using UnityEngine;
using Winch;


public class PhysicsBody : IUpdateable
{
    public Parameter<Vector3> Position { get; private set; } = new Parameter<Vector3>();
    public Parameter<Vector3> Velocity { get; private set; } = new Parameter<Vector3>();
    public Parameter<Vector3> Force { get; private set; } = new Parameter<Vector3>();

    public void Update(UpdateInfo info)
    {
        Velocity.Value += Force.Value * info.TimeStep;
        Position.Value += Velocity.Value * info.TimeStep;
    }
}
