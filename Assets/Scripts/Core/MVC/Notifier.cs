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
            if (events.ContainsKey(eventType.ToString()))
            {
                events[eventType.ToString()] += func;
                return;
            }
            events[eventType.ToString()] = func;
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
            if (events.ContainsKey(eventType.ToString()))
            {
                events[eventType.ToString()] -= func;
            }
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
            VariadicDelegate func = null;
            if (events.TryGetValue(eventType.ToString(), out func))
            {
                if (func != null)
                {
                    func(e);
                }
            }
        }

        public void RemoveAllEventHandler(EventType eventType)
        {
            if(events.ContainsKey(eventType.ToString()))
            {
                if(events[eventType.ToString()] != null)
                {
                    Delegate[] arr = events[eventType.ToString()].GetInvocationList();
                    for(int i = 0; i < arr.Length; i++)
                    {
                        VariadicDelegate func = arr[i] as VariadicDelegate;
                        RemoveEventHandler(eventType.ToString(), func);
                    }
                }
            }
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
