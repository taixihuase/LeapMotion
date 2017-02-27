using Define;
using System;
using System.Collections;
using System.Collections.Generic;
using Tool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Core.Manager
{
    public class SceneAction
    {
        public UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.Scene> ActiveSceneChanged;

        public UnityAction<UnityEngine.SceneManagement.Scene, LoadSceneMode> SceneLoaded;

        public UnityAction<UnityEngine.SceneManagement.Scene> SceneUnloaded;
    }


    public sealed class SceneManager : Singleton<SceneManager>
    {
        private Dictionary<string, SceneAction> sceneActions = new Dictionary<string, SceneAction>();

        private string currentSceneName = string.Empty;

        public string CurrentSceneName
        {
            get { return currentSceneName; }
        }

        private int currentSceneId = -1;

        public int CurrentSceneId
        {
            get { return currentSceneId; }
        }

        private SceneType currentScene;

        public SceneType CurrentScene
        {
            get { return currentScene; }
            private set
            {
                currentScene = value;
                currentSceneName = EnumDescriptionTool.GetDescription(value);
                currentSceneId = (int)value;
            }
        }

        private string nextSceneName = string.Empty;

        public string NextSceneName
        {
            get { return nextSceneName; }
        }

        private int nextSceneId = -1;

        public int NextSceneId
        {
            get { return nextSceneId; }
        }

        private SceneType nextScene;

        public SceneType NextScene
        {
            get { return nextScene; }
            private set
            {
                nextScene = value;
                nextSceneName = EnumDescriptionTool.GetDescription(value);
                nextSceneId = (int)value;
            }
        }

        private UnityAction<UnityEngine.SceneManagement.Scene, LoadSceneMode> singleLoaded;

        private List<SceneType> openScenes = new List<SceneType>();
        
        public SceneManager()
        {
            CurrentScene = EnumDescriptionTool.GetEnum<SceneType>(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            openScenes.Add(currentScene);
            NextScene = SceneType.None;

            UnityAction <UnityEngine.SceneManagement.Scene, LoadSceneMode> sceneLoaded = (scene, mode) =>
            {
                if (mode == LoadSceneMode.Single)
                {
                    CurrentScene = NextScene;

                    for (int i = 0; i < openScenes.Count; i++)
                    {
                        UIManager.Instance.CloseSceneWindows(openScenes[i]);
                    }
                    openScenes.Clear();
                    openScenes.Add(currentScene);
                }
                else
                {
                    openScenes.Add(nextScene);
                }
                NextScene = SceneType.None;

                if(sceneActions.ContainsKey(scene.name) && sceneActions[scene.name].SceneLoaded != null)
                {
                    Delegate[] loaded = sceneActions[scene.name].SceneLoaded.GetInvocationList();
                    for(int i = 0; i < loaded.Length; i++)
                    {
                        UnityAction<UnityEngine.SceneManagement.Scene, LoadSceneMode> func = loaded[i] as UnityAction<UnityEngine.SceneManagement.Scene, LoadSceneMode>;
                        func(scene, mode);
                    }
                }
            };

            UnityAction<UnityEngine.SceneManagement.Scene> sceneUnloaded = (scene) =>
            {
                if (sceneActions.ContainsKey(scene.name) && sceneActions[scene.name].SceneUnloaded != null)
                {
                    Delegate[] unloaded = sceneActions[scene.name].SceneUnloaded.GetInvocationList();
                    for (int i = 0; i < unloaded.Length; i++)
                    {
                        UnityAction<UnityEngine.SceneManagement.Scene> func = unloaded[i] as UnityAction<UnityEngine.SceneManagement.Scene>;
                        func(scene);
                    }
                }
                ClearLoadedEventHandler(CurrentScene);
                ClearUnloadedEventHandler(CurrentScene);
                ClearChangedEventHandler(CurrentScene);
            };

            UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.Scene> activeSceneChanged = (scene, toScene) =>
            {
                if (sceneActions.ContainsKey(toScene.name) && sceneActions[toScene.name].ActiveSceneChanged != null)
                {
                    Delegate[] changed = sceneActions[toScene.name].ActiveSceneChanged.GetInvocationList();
                    for (int i = 0; i < changed.Length; i++)
                    {
                        UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.Scene> func = changed[i] as UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.Scene>;
                        func(scene, toScene);
                    }
                }
            };

            SceneAction action = new SceneAction();
            action.SceneLoaded += sceneLoaded;
            action.SceneUnloaded += sceneUnloaded;
            action.ActiveSceneChanged += activeSceneChanged;
            sceneActions.Add("SceneManager", action);

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += action.SceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += action.SceneUnloaded;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += action.ActiveSceneChanged;
        }

        #region Event Handler

        public bool IsContainSceneEventHandler(SceneType scene)
        {
            return sceneActions.ContainsKey(scene.GetDescription());
        }

        public void AddLoadedEventHandler(SceneType scene, UnityAction<UnityEngine.SceneManagement.Scene, LoadSceneMode> sceneLoaded)
        {
            string name = scene.GetDescription();
            if(sceneActions.ContainsKey(name) == false)
            {
                sceneActions.Add(name, new SceneAction());
            }
            sceneActions[name].SceneLoaded += sceneLoaded;
            Debug.Log(string.Format("Scene \"{0}\" Add Loaded", name));
        }

        public void RemoveLoadedEventHandler(SceneType scene, UnityAction<UnityEngine.SceneManagement.Scene, LoadSceneMode> sceneLoaded)
        {
            string name = scene.GetDescription();
            if(sceneActions.ContainsKey(name))
            {
                sceneActions[name].SceneLoaded -= sceneLoaded;
                Debug.Log(string.Format("Scene \"{0}\" Remove Loaded", name));
            }
        }

        public void AddUnloadedEventHandler(SceneType scene, UnityAction<UnityEngine.SceneManagement.Scene> sceneUnloaded)
        {
            string name = scene.GetDescription();
            if (sceneActions.ContainsKey(name) == false)
            {
                sceneActions.Add(name, new SceneAction());
            }
            sceneActions[name].SceneUnloaded += sceneUnloaded;
            Debug.Log(string.Format("Scene \"{0}\" Add Unloaded", name));
        }

        public void RemoveUnloadedEventHandler(SceneType scene, UnityAction<UnityEngine.SceneManagement.Scene> sceneUnloaded)
        {
            string name = scene.GetDescription();
            if (sceneActions.ContainsKey(name))
            {
                sceneActions[name].SceneUnloaded -= sceneUnloaded;
                Debug.Log(string.Format("Scene \"{0}\" Remove Unloaded", name));
            }
        }
        
        public void AddChangedEventHandler(SceneType scene, UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.Scene> changed)
        {
            string name = scene.GetDescription();
            if (sceneActions.ContainsKey(name) == false)
            {
                sceneActions.Add(name, new SceneAction());
            }
            sceneActions[name].ActiveSceneChanged += changed;
            Debug.Log(string.Format("Scene \"{0}\" Add Changed", name));
        }

        public void RemoveChangedEventHandler(SceneType scene, UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.Scene> changed)
        {
            string name = scene.GetDescription();
            if (sceneActions.ContainsKey(name))
            {
                sceneActions[name].ActiveSceneChanged -= changed;
                Debug.Log(string.Format("Scene \"{0}\" Remove Changed", name));
            }
        }

        public void ClearLoadedEventHandler(SceneType scene)
        {
            string name = scene.GetDescription();
            if (sceneActions.ContainsKey(name) && sceneActions[name].SceneLoaded != null)
            {
                Debug.Log(string.Format("Scene \"{0}\" Clear Loaded", name));
                sceneActions[name].SceneLoaded = null;
            }
        }

        public void ClearUnloadedEventHandler(SceneType scene)
        {
            string name = scene.GetDescription();
            if (sceneActions.ContainsKey(name) && sceneActions[name].SceneUnloaded != null)
            {
                Debug.Log(string.Format("Scene \"{0}\" Clear Unloaded", name));
                sceneActions[name].SceneUnloaded = null;
            }
        }

        public void ClearChangedEventHandler(SceneType scene)
        {
            string name = scene.GetDescription();
            if (sceneActions.ContainsKey(name) && sceneActions[name].ActiveSceneChanged != null)
            {
                Debug.Log(string.Format("Scene \"{0}\" Clear Changed", name));
                sceneActions[name].ActiveSceneChanged = null;
            }
        }

        public void RemoveAllEventHandler(SceneType scene)
        {
            sceneActions.Remove(scene.GetDescription());
        }

        #endregion

        public void LoadScene(SceneType scene, LoadSceneMode loadMode = LoadSceneMode.Single, UnityAction<UnityEngine.SceneManagement.Scene, LoadSceneMode> sceneLoaded = null)
        {
            if (sceneLoaded != null)
            {
                AddLoadedEventHandler(scene, sceneLoaded);
            }

            NextScene = scene;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene.GetDescription(), loadMode);
        }

        public void LoadSceneAsync(SceneType scene, LoadSceneMode loadMode = LoadSceneMode.Single, UnityAction<UnityEngine.SceneManagement.Scene, LoadSceneMode> sceneLoaded = null)
        {
            if (sceneLoaded != null)
            {
                AddLoadedEventHandler(scene, sceneLoaded);
            }

            NextScene = scene;
            CoroutineManager.Instance.StartCoroutine(LoadSceneAsync(scene.GetDescription(), loadMode));
        }

        private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadMode);
            async.allowSceneActivation = false;
            while (async.progress < 0.9f)
            {
                yield return new WaitForEndOfFrame();
            }
            async.allowSceneActivation = true;
        }
    }
}