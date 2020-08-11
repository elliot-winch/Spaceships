using UnityEngine;

namespace Winch
{
    public struct UpdateInfo
    {
        public int Time;
        public float TicksPerUpdate;
    }

    public class Updater : MonoBehaviour
    {
        [SerializeField]
        private int m_StartingTimeScale;

        public Parameter<int> CurrentTick = new Parameter<int>();
        public Parameter<int> TicksPerUpdate;

        public UniqueList<IUpdateable> Updatables = new UniqueList<IUpdateable>();

        private void Awake()
        {
            TicksPerUpdate = new Parameter<int>(m_StartingTimeScale);
        }

        private void FixedUpdate()
        {
            CurrentTick.Value += TicksPerUpdate.Value;

            foreach (IUpdateable updateable in Updatables.Values)
            {
                updateable.Update(new UpdateInfo()
                {
                    TicksPerUpdate = TicksPerUpdate.Value,
                    Time = CurrentTick.Value
                });
            }
        }
    }
}
