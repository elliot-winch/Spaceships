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
    private int m_SwitchTime;

    private float N = 0.45f;

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
        body.Position.Value = path.Points[0].Position;
        body.Velocity.Value = m_StartVelocity;

        int targetPointIndex = 0;

        for(int time = 0; time < path.Time; time++)
        {
            TimeSpacePoint current = path.Points[targetPointIndex];

            if (time >= current.Time)
            {
                TimeSpacePoint target = path.Points[targetPointIndex + 1];
                int timeDelta = target.Time - current.Time;

                //!! Issue: have to round here, might lost accuracy
                int firstTimeDelta = Mathf.RoundToInt(N * timeDelta);
                int secondTimeDelta = timeDelta - firstTimeDelta;

                Vector3 vMid = CalculateVMid(N, target.Position - current.Position, timeDelta, body.Velocity.Value, target.Velocity);

                Debug.Log(target.Position - current.Position);
                Debug.Log(vMid);

                m_CurrentPlan = new AccelerationPlan()
                {
                    FirstLeg = (vMid - body.Velocity.Value) / firstTimeDelta,
                    SecondLeg = (target.Velocity - vMid) / secondTimeDelta
                };

                m_SwitchTime = firstTimeDelta + current.Time;

                body.Acceleration.Value = m_CurrentPlan.FirstLeg;    

                targetPointIndex++;
            }

            if(time >= m_SwitchTime)
            {
                body.Acceleration.Value = m_CurrentPlan.SecondLeg;
            }

            Debug.DrawRay(body.Position.Value, body.Acceleration.Value.normalized, Color.red, 100000f);

            body.Update(new UpdateInfo()
            {
                TicksPerUpdate = 1,
                Time = time,
            });

            Debug.Log(time + " " + body.Velocity.Value.ToString("F5") + " " + body.Position.Value.ToString("F5"));
            m_Positions[time] = body.Position.Value;
        }
    }

    /*
    private Vector3 ResolveForTime(Vector3 positionDelta, Vector3 initialVelocity, float timeDelta)
    {
        return 2 * (positionDelta - initialVelocity * timeDelta) / (timeDelta * timeDelta);
    }
    */

    private Vector3 CalculateVMid(float n, Vector3 positionDelta, int timeDelta, Vector3 initialVelocity, Vector3 targetVelocity)
    {
        return 2 * positionDelta / timeDelta + (n - 1) * targetVelocity - n * initialVelocity;
    }
}
