using System.Linq;

namespace Winch.Spaceships
{
    public abstract class RoutePlanner
    {
        private Path m_Path;
        private SpaceTimePoint[] m_Positions;

        public SpaceTimePoint SamplePosition(int time)
        {
            int index = time - m_Path.StartTime;
            return index < m_Positions.Length ? m_Positions[index] : m_Positions.Last();
        }

        public void CreateRoute(Path path)
        {
            m_Path = path;
            m_Positions = Create(path);
        }

        protected abstract SpaceTimePoint[] Create(Path path);
    }
}