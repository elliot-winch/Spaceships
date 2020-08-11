using UnityEngine;
using Winch;

public class ShipMovementTest : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_StartingVelocity;
    [SerializeField]
    private TimeSpacePoint[] m_Points;

    [Space(5)]
    [SerializeField]
    private Updater m_Updater;
    [SerializeField]
    private Transform m_TestVisuals;

    void Start()
    {
        ShipMovement ship = new ShipMovement();
        ship.PhysicsBody.Velocity.Value = m_StartingVelocity;
        ship.StartUpdate(m_Updater);

        ShipMovementEvent evt = new ShipMovementEvent(0, "Test", new Path(0, m_Points));

        ship.ReceiveEvent(evt);

        ship.PhysicsBody.Position.Subscribe(pos => m_TestVisuals.position = pos);

        foreach(var p in m_Points)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = p.Position;
        }
        //ship.PhysicsBody.Position.Subscribe(pos => Debug.Log(pos.ToString("F4")));
        //ship.PhysicsBody.Velocity.Subscribe(vel => Debug.Log(vel.ToString("F4")));
    }
}
