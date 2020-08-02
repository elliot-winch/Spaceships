using UnityEngine;

namespace Winch.Update
{
    public class Updater : MonoBehaviour
    {
        public static Updater Instance { get; private set; }

        public Parameter<int> CurrentTick = new Parameter<int>();
        public Parameter<float> TimeScale = new Parameter<float>();
        public UniqueList<IUpdateable> Updatables = new UniqueList<IUpdateable>();

        private void Awake()
        {
            if(Instance != null)
            {
                Debug.LogError("Updater instance already created");
            }

            Instance = this;
        }

        private void FixedUpdate()
        {
            foreach(IUpdateable updateable in Updatables.Values)
            {
                updateable.Update(new UpdateInfo()
                {
                    TimeStep = TimeScale.Value,
                    Tick = CurrentTick.Value
                });
            }
        }
    }
}
