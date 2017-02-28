using Core.MVC;
using Define;

namespace Core.Manager
{
    public sealed class EventManager : Singleton<EventManager>, INotifier
    {
        private Notifier notifier = new Notifier();

        public void AddEventHandler(string eventName, VariadicDelegate func)
        {
            notifier.AddEventHandler(eventName, func);
        }

        public void AddEventHandler(EventType eventType, VariadicDelegate func)
        {
            notifier.AddEventHandler(eventType, func);
        }

        public void ClearEventHandler()
        {
            notifier.ClearEventHandler();
        }

        public void RaiseEvent(string eventName, params object[] e)
        {
            notifier.RaiseEvent(eventName, e);
        }

        public void RaiseEvent(EventType eventType, params object[] e)
        {
            notifier.RaiseEvent(eventType, e);
        }

        public void RemoveAllEventHandler(string eventName)
        {
            notifier.RemoveAllEventHandler(eventName);
        }

        public void RemoveAllEventHandler(EventType eventType)
        {
            notifier.RemoveAllEventHandler(eventType);
        }

        public void RemoveEventHandler(string eventName, VariadicDelegate func)
        {
            notifier.RemoveEventHandler(eventName, func);
        }

        public void RemoveEventHandler(EventType eventType, VariadicDelegate func)
        {
            notifier.RemoveEventHandler(eventType, func);
        }
    }
}
