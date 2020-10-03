using System.Linq;

namespace Winch.Spaceships
{
    public abstract class RoutePlanner
    {
        private Path m_Path;

        public Parameter<SpaceTimePoint[]> Route { get; private set; } = new Parameter<SpaceTimePoint[]>();

        public SpaceTimePoint SamplePosition(int time)
        {
            int index = time - m_Path.StartTime;
            return index < Route.Value.Length ? Route.Value[index] : Route.Value.Last();
        }

        public void CreateRoute(Path path)
        {
            m_Path = path;
            Route.Value = Create(path);
        }

        protected abstract SpaceTimePoint[] Create(Path path);
    }
}