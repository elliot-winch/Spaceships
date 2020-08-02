using Winch.Update;

namespace Winch
{
    public interface IUpdateable
    {
        void Update(UpdateInfo updater);
    }

    public struct UpdateInfo
    {
        public int Tick;
        public float TimeStep;
    }

    public static class UpdateableExtension
    {
        public static void BeginUpdate(this IUpdateable updateable)
        {
            Updater.Instance.Updatables.Add(updateable);
        }

        public static void EndUpdate(this IUpdateable updateable)
        {
            Updater.Instance.Updatables.Remove(updateable);
        }
    }
}