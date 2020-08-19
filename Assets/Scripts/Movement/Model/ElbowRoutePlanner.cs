using UnityEngine;

namespace Winch.Spaceships
{
    public class ElbowRoutePlanner : RoutePlanner
    {
        private struct AccelerationPlan
        {
            public Vector3 FirstLeg;
            public Vector3 SecondLeg;
        }

        private AccelerationPlan m_CurrentPlan;

        private float m_ElbowPoint;

        public ElbowRoutePlanner(float elbowPoint)
        {
            m_ElbowPoint = Mathf.Clamp(elbowPoint, 0, 1);
        }

        protected override SpaceTimePoint[] Create(Path path)
        {
            SpaceTimePoint[] positions = new SpaceTimePoint[path.Time];

            PhysicsBody body = new PhysicsBody();
            int switchTime = 0;

            //The first point on the path is the start position / velocity
            body.SetFromSpaceTimePoint(path.Points[0]);

            int targetPointIndex = 0;

            for (int time = 0; time < path.Time; time++)
            {
                SpaceTimePoint current = path.Points[targetPointIndex];

                if (time >= current.Time)
                {
                    SpaceTimePoint target = path.Points[targetPointIndex + 1];
                    int timeDelta = target.Time - current.Time;

                    //!! Issue: have to round here, might lost accuracy
                    int firstTimeDelta = Mathf.RoundToInt(m_ElbowPoint * timeDelta);
                    int secondTimeDelta = timeDelta - firstTimeDelta;

                    Vector3 vMid = CalculateVMid(m_ElbowPoint, target.Position - current.Position, timeDelta, body.Velocity.Value, target.Velocity);

                    m_CurrentPlan = new AccelerationPlan()
                    {
                        FirstLeg = (vMid - body.Velocity.Value) / firstTimeDelta,
                        SecondLeg = (target.Velocity - vMid) / secondTimeDelta
                    };

                    switchTime = firstTimeDelta + current.Time;

                    body.Acceleration.Value = m_CurrentPlan.FirstLeg;

                    targetPointIndex++;
                }

                if (time >= switchTime)
                {
                    body.Acceleration.Value = m_CurrentPlan.SecondLeg;
                }

                body.Update(new UpdateInfo()
                {
                    TicksPerUpdate = 1,
                    Time = time,
                });

                SpaceTimePoint point = body.GetSpaceTimePoint();
                point.Time = time;

                positions[time] = point;
            }

            return positions;
        }

        private Vector3 CalculateVMid(float n, Vector3 positionDelta, int timeDelta, Vector3 initialVelocity, Vector3 targetVelocity)
        {
            return 2 * positionDelta / timeDelta + (n - 1) * targetVelocity - n * initialVelocity;
        }
    }
}
