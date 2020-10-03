using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Winch.Spaceships
{
    public class ShipMovementView : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer m_LineRenderer;
        [SerializeField]
        private int m_SampleEveryNFrames;
        [SerializeField]
        private Color m_MinSpeedColor;
        [SerializeField]
        private Color m_MaxSpeedColor;
        [SerializeField]
        private float m_MaxVelocity;

        private ShipMovement m_Model;

        public ShipMovement Model
        {
            get => m_Model;
            set
            {
                if(m_Model != null)
                {
                    m_Model.RoutePlanner.Route.Unsubscribe(CreateView);
                }

                m_Model = value;
                m_Model.RoutePlanner.Route.Subscribe(CreateView);
            }
        }

        private void CreateView(SpaceTimePoint[] points)
        {
            if(points != null)
            {
                m_LineRenderer.enabled = true;

                SetLineRenderer(points.SampleBookended(m_SampleEveryNFrames));
            }
            else
            {
                m_LineRenderer.enabled = false;
            }
        }

        private void SetLineRenderer(IEnumerable<SpaceTimePoint> points)
        {
            m_LineRenderer.positionCount = points.Count();
            m_LineRenderer.SetPositions(points.Select(point => point.Position).ToArray());


            Gradient gradient = new Gradient()
            {
                colorKeys = points.Select((point, index) =>
                {
                    return new GradientColorKey()
                    {
                        color = VelocityToColor(point.Velocity),
                        time = index / (float)points.Count()
                    };
                }).SampleCardinal(4).ToArray(),
            };
            
            m_LineRenderer.colorGradient = gradient;
        }

        private Color VelocityToColor(Vector3 velocity)
        {
            return Color.Lerp(m_MinSpeedColor, m_MaxSpeedColor, velocity.magnitude / m_MaxVelocity);
        }
    }
}
