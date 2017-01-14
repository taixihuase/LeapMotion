using Define;

namespace Core.MVC
{
    public interface INotifier
    {
        void AddEventHandler(string eventName, VariadicDelegate func);

        void RemoveEventHandler(string eventName, VariadicDelegate func);

        void RaiseEvent(string eventName, params object[] e);

        void RemoveAllEventHandler(string eventName);

        void ClearEventHandler();
    }
}
