using Define;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.MVC
{
    public class Notifier : INotifier
    {
        private Dictionary<string, VariadicDelegate> events = new Dictionary<string, VariadicDelegate>();

        public void AddEventHandler(string eventName, VariadicDelegate func)
        {
            if(events.ContainsKey(eventName))
            {
                events[eventName] += func;
                return;
            }
            events[eventName] = func;
        }

        public void AddEventHandler(EventType eventType, VariadicDelegate func)
        {
            AddEventHandler(eventType.ToString(), func);
        }

        public void RemoveEventHandler(string eventName, VariadicDelegate func)
        {
            if(events.ContainsKey(eventName))
            {
                events[eventName] -= func;
            }
        }

        public void RemoveEventHandler(EventType eventType, VariadicDelegate func)
        {
            RemoveEventHandler(eventType.ToString(), func);
        }

        public void RaiseEvent(string eventName, params object[] e)
        {
            VariadicDelegate func = null;
            if(events.TryGetValue(eventName, out func))
            {
                if(func != null)
                {
                    func(e);
                }
            }
        }

        public void RaiseEvent(EventType eventType, params object[] e)
        {
            RaiseEvent(eventType.ToString(), e);
        }

        public void RemoveAllEventHandler(EventType eventType)
        {
            RemoveAllEventHandler(eventType.ToString());
        }

        public void RemoveAllEventHandler(string eventName)
        {
            if (events.ContainsKey(eventName))
            {
                if (events[eventName] != null)
                {
                    Delegate[] arr = events[eventName].GetInvocationList();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        VariadicDelegate func = arr[i] as VariadicDelegate;
                        RemoveEventHandler(eventName, func);
                    }
                }
            }
        }

        public void ClearEventHandler()
        {
            if(events.Count == 0)
            {
                return;
            }
            string[] arr = events.Keys.ToArray();
            for(int i = 0; i < arr.Length; i++)
            {
                RemoveAllEventHandler(arr[i]);
            }
        }
    }
}
