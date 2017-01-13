using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.MVC
{
    public class Notifier
    {
        public delegate void MyDelegate(params object[] args);

        private Dictionary<string, MyDelegate> events = new Dictionary<string, MyDelegate>();

        public void AddEventHandler(string eventName, MyDelegate func)
        {
            if(events.ContainsKey(eventName))
            {
                events[eventName] += func;
                return;
            }
            events[eventName] = func;
        }

        public void RemoveEventHandler(string eventName, MyDelegate func)
        {
            if(events.ContainsKey(eventName))
            {
                events[eventName] -= func;
            }
        }

        public void RaiseEvent(string eventName, params object[] e)
        {
            MyDelegate func = null;
            if(events.TryGetValue(eventName, out func))
            {
                if(func != null)
                {
                    func(e);
                }
            }
        }

        public void RemoveAllEventHandler(string eventName)
        {
            if(events.ContainsKey(eventName))
            {
                if(events[eventName] != null)
                {
                    Delegate[] arr = events[eventName].GetInvocationList();
                    for(int i = 0; i < arr.Length; i++)
                    {
                        MyDelegate func = arr[i] as MyDelegate;
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
