using System;
using System.Collections.Generic;
using UnityEngine;
using Define;

namespace Core.Manager
{
    public sealed class FrameManager : MonoSingleton<FrameManager>
    {
        private event Action onUpdate;

        private event Action onFixedUpdate;

        private event Action onLateUpdate;

        private Dictionary<string, Action> nameToFunctionMap = new Dictionary<string, Action>();

        private Dictionary<Action, Dictionary<UpdateType, List<string>>> functionToNameMap = new Dictionary<Action, Dictionary<UpdateType, List<string>>>();

        public bool CheckNameInMap(string name)
        {
            if (name != null)
            {
                if (nameToFunctionMap.ContainsKey(name))
                {
                    Debug.LogWarning("已存在名为\"" + name + "\"的委托方法");
                    return true;
                }
            }
            return false;
        }

        private void AddFunctionToMap(Action func, UpdateType updateType, string name)
        {
            nameToFunctionMap.Add(name, func);
            if (functionToNameMap.ContainsKey(func) == false)
            {
                functionToNameMap.Add(func, new Dictionary<UpdateType, List<string>>());
            }
            if (functionToNameMap[func].ContainsKey(updateType) == false)
            {
                functionToNameMap[func].Add(updateType, new List<string>());
            }
            functionToNameMap[func][updateType].Add(name);
        }

        public string RegisterUpdate(Action func, string name = null)
        {
            if(CheckNameInMap(name))
            {
                return null;
            }
            else if(name == null)
            {
                name = Guid.NewGuid().ToString();
            }
            onUpdate += func;
            AddFunctionToMap(func, UpdateType.Update, name);
            return name;
        }

        public string RegisterFixedUpdate(Action func, string name = null)
        {
            if (CheckNameInMap(name))
            {
                return null;
            }
            else if (name == null)
            {
                name = Guid.NewGuid().ToString();
            }
            onFixedUpdate += func;
            AddFunctionToMap(func, UpdateType.FixedUpdate, name);
            return name;
        }

        public string RegisterLateUpdate(Action func, string name = null)
        {
            if (CheckNameInMap(name))
            {
                return null;
            }
            else if (name == null)
            {
                name = Guid.NewGuid().ToString();
            }
            onLateUpdate += func;
            AddFunctionToMap(func, UpdateType.LateUpdate, name);
            return name;
        }

        private void RemoveFunctionFromMap(Action func, UpdateType updateType, string name)
        {
            List<string> remove = null;
            if (functionToNameMap.ContainsKey(func) && functionToNameMap[func].ContainsKey(updateType))
            {
                List<string> list = functionToNameMap[func][updateType];
                if(list.Count > 0)
                {
                    remove = new List<string>();
                    if (name != null)
                    {
                        remove.Add(name);
                        list.Remove(name);
                    }
                    else
                    {
                        remove.AddRange(list);
                    }
                }
            }

            if(remove != null)
            {
                for(int i = 0; i < remove.Count; i++)
                {
                    nameToFunctionMap.Remove(remove[i]);
                }
            }
        }

        public void UnRegisterUpdate(Action func, string name = null)
        {
            RemoveFunctionFromMap(func, UpdateType.Update, name);
            onUpdate -= func;
        }

        public void UnRegisterFixedUpdate(Action func, string name = null)
        {
            RemoveFunctionFromMap(func, UpdateType.FixedUpdate, name);
            onFixedUpdate -= func;
        }

        public void UnRegisterLateUpdate(Action func, string name = null)
        {
            RemoveFunctionFromMap(func, UpdateType.LateUpdate, name);
            onLateUpdate -= func;
        }
    }
}
