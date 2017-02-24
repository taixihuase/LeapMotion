using Define;
using System;
using UnityEngine;
using Tool;
using System.Collections.Generic;
using System.Linq;
using Leap.Unity;
using Leap.Unity.InputModule;
using UnityEngine.EventSystems;

namespace Core.Manager
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private GameObject uiCamera;

        public GameObject UICamera
        {
            get { return uiCamera; }
        }

        private Transform root;

        public Transform Root
        {
            get { return root; }
        }

        private GameObject eventSystem;

        public GameObject EventSystem
        {
            get { return eventSystem; }
        }


        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            root = transform;
            root.localPosition = Vector3.zero;
        }

        private Dictionary<SceneType, Dictionary<WindowType, UnityEngine.Object>> openWindows = new Dictionary<SceneType, Dictionary<WindowType, UnityEngine.Object>>(new EnumComparer<SceneType>());
        
        public void OpenWindow(SceneType scene, WindowType win, Action<UnityEngine.Object> callback = null, bool isAsync = false, bool fromServer = false)
        {
            Action<UnityEngine.Object> act = (obj) =>
            {
                GameObject go = obj as GameObject;
                if (go != null)
                {
                    GameObject instance = Instantiate(go);
                    instance.transform.parent = Root;

                    if(openWindows.ContainsKey(scene) == false)
                    {
                        openWindows.Add(scene, new Dictionary<WindowType, UnityEngine.Object>(new EnumComparer<WindowType>()));
                    }
                    if(openWindows[scene].ContainsKey(win) == false)
                    {
                        openWindows[scene].Add(win, instance);
                    }

                    if(callback != null)
                    {
                        callback(instance);
                    }
                }
            };

            ResourceManager.Instance.LoadAsset(ResourceType.UI, win.GetDescription(), act, isAsync, fromServer);
        }

        public void CloseWindow(SceneType scene, WindowType win, Action<UnityEngine.Object> callback = null)
        {
            UnityEngine.Object obj = null;
            Dictionary<WindowType, UnityEngine.Object> dict = null;
            if(openWindows.TryGetValue(scene, out dict) && dict.TryGetValue(win, out obj))
            {
                if(callback != null)
                {
                    callback(obj);
                }
                openWindows[scene].Remove(win);
                Destroy(obj);
            }
        }

        public void CloseSceneWindows(SceneType scene, Action<UnityEngine.Object> callback = null)
        {
            Dictionary<WindowType, UnityEngine.Object> dict = null;
            if (openWindows.TryGetValue(scene, out dict))
            {
                var list = dict.Values.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    UnityEngine.Object obj = list[i];
                    if (callback != null)
                    {
                        callback(obj);
                    }
                    Destroy(obj);
                }
                openWindows.Remove(scene);
            }
        }
    }
}
