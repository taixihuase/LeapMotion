using Define;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.MVC
{
    public abstract class View : MonoBehaviour
    {
        protected Model model;

        Dictionary<string, VariadicDelegate> events = new Dictionary<string, VariadicDelegate>();

        public virtual void Init(Model model)
        {
            this.model = model;
        }

        protected void Bind(Enum attribute, VariadicDelegate func)
        {
            if(model != null)
            {
                string key = string.Format("{0}{1}", model.Name, attribute);
                if(events.ContainsKey(key))
                {
                    model.RemoveEventHandler(key, events[key]);
                    events.Remove(key);
                }
                model.AddEventHandler(key, func);
                events.Add(key, func);
            }
        }

        protected void UnBind(Enum attribute, VariadicDelegate func)
        {
            if(model != null)
            {
                string key = string.Format("{0}{1}", model.Name, attribute);
                if(events.ContainsKey(key))
                {
                    model.RemoveEventHandler(key, func);
                    events.Remove(key);
                }
            }
        }

        private void ClearBind()
        {
            if(model != null)
            {
                if(events.Count > 0)
                {
                    var iter = events.GetEnumerator();
                    while(iter.MoveNext())
                    {
                        model.RemoveEventHandler(iter.Current.Key, iter.Current.Value);
                    }
                    iter.Dispose();
                }
                model = null;
                events.Clear();
            }
        }

        public void Reset()
        {
            ClearBind();
        }

        protected virtual void Awake()
        {

        }

        protected virtual void OnDestroy()
        {
            ClearBind();
        }

        protected virtual void Update()
        {

        }
    }
}