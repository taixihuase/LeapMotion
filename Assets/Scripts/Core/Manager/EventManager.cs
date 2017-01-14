using Core.MVC;
using Define;

namespace Core.Manager
{
    public class EventManager : Singleton<EventManager>, INotifier
    {
        private Notifier notifier = new Notifier();

        public void AddEventHandler(string eventName, VariadicDelegate func)
        {
            notifier.AddEventHandler(eventName, func);
        }

        public void ClearEventHandler()
        {
            notifier.ClearEventHandler();
        }

        public void RaiseEvent(string eventName, params object[] e)
        {
            notifier.RaiseEvent(eventName, e);
        }

        public void RemoveAllEventHandler(string eventName)
        {
            notifier.RemoveAllEventHandler(eventName);
        }

        public void RemoveEventHandler(string eventName, VariadicDelegate func)
        {
            notifier.RemoveEventHandler(eventName, func);
        }
    }
}
