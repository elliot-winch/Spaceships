using System.Linq;
using UnityEngine;
using Winch;

public interface IRoute
{
    void CreateRoute(Path path);

    Vector3 SamplePosition(int time);
}

public class SimpleAccelerationRoute : IRoute
{
    private struct AccelerationPlan
    {
        public Vector3 FirstLeg;
        public Vector3 SecondLeg;
    }

    private Vector3[] m_Positions;
    private Vector3 m_StartVelocity;
    private Path m_Path;
    private AccelerationPlan m_CurrentPlan;

    public SimpleAccelerationRoute(Vector3 startVelocity)
    {
        m_StartVelocity = startVelocity;
    }

    Vector3 IRoute.SamplePosition(int time)
    {
        int index = time - m_Path.StartTime;
        return index < m_Positions.Length ? m_Positions[index] : m_Positions.Last();
    }

    void IRoute.CreateRoute(Path path)
    {
        m_Path = path;
        m_Positions = new Vector3[path.Time];

        PhysicsBody body = new PhysicsBody();
        body.Velocity.Value = m_StartVelocity;

        int targetPointIndex = 0;

        for(int time = 0; time < path.Time; time++)
        {
            if(time >= path.Points[targetPointIndex].Time)
            {
                TimeSpacePoint current = path.Points[targetPointIndex];
                TimeSpacePoint target = path.Points[targetPointIndex + 1];

                //if (target.UseVelocity)
                //{
                m_CurrentPlan = ElbowResolve(0.5f, target.Position - current.Position, target.Time - current.Time, body.Velocity.Value, target.Velocity);
                body.Acceleration.Value = m_CurrentPlan.FirstLeg;    
                //}
                /*
                else
                {
                    Vector3 positionDelta = target.Position - current.Position;
                    float timeDelta = target.Time - current.Time;
                    body.Acceleration.Value = ResolveForTime(positionDelta, body.Velocity.Value, timeDelta);
                }
                */

                Debug.DrawRay(body.Position.Value, body.Acceleration.Value * 100f, Color.red, 100000f);

                targetPointIndex++;
            }

            body.Update(new UpdateInfo()
            {
                TicksPerUpdate = 1,
                Time = time,
            });

            Debug.Log(body.Velocity.Value.ToString("F5"));
            m_Positions[time] = body.Position.Value;
        }
    }

    private Vector3 ResolveForTime(Vector3 positionDelta, Vector3 initialVelocity, float timeDelta)
    {
        return 2 * (positionDelta - initialVelocity * timeDelta) / (timeDelta * timeDelta);
    }

    private AccelerationPlan ElbowResolve(float n, Vector3 positionDelta, int timeDelta, Vector3 initialVelocity, Vector3 targetVelocity)
    {
        Vector3 vMid = (positionDelta / (2 * timeDelta) + (n - 1) * targetVelocity + n * initialVelocity) / (2 * n - 1);

        return new AccelerationPlan()
        {
            FirstLeg = n * timeDelta * (vMid - initialVelocity),
            SecondLeg = n * timeDelta * (targetVelocity - vMid)
        };
    }
}
