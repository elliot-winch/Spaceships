
namespace Winch
{
    public interface IUpdateable
    {
        void Update(UpdateInfo updateInfo);
    }

    public static class UpdateableExtension
    {
        public static void StartUpdate(this IUpdateable updateable, Updater updater)
        {
            updater.Updatables.Add(updateable);
        }

        public static void EndUpdate(this IUpdateable updateable, Updater updater)
        {
            updater.Updatables.Remove(updateable);
        }
    }
}