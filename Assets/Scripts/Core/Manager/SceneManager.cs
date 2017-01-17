using Define;
using System;
using System.Collections.Generic;
using Tool;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Core.Manager
{
    public class SceneAction
    {
        public UnityAction<Scene, Scene> ActiveSceneChanged;

        public UnityAction<Scene, LoadSceneMode> SceneLoaded;

        public UnityAction<Scene> SceneUnloaded;
    }


    public class SceneManager : Singleton<SceneManager>
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
        
        public SceneManager()
        {
            CurrentScene = EnumDescriptionTool.GetEnum<SceneType>(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            NextScene = SceneType.None;

            UnityAction <Scene, LoadSceneMode> sceneLoaded = (scene, mode) =>
            {
                if (mode != LoadSceneMode.Additive)
                {
                    CurrentScene = EnumDescriptionTool.GetEnum<SceneType>(scene.name);
                }
                NextScene = SceneType.None;

                if(sceneActions.ContainsKey(scene.name) && sceneActions[scene.name].SceneLoaded != null)
                {
                    Delegate[] loaded = sceneActions[scene.name].SceneLoaded.GetInvocationList();
                    for(int i = 0; i < loaded.Length; i++)
                    {
                        UnityAction<Scene, LoadSceneMode> func = loaded[i] as UnityAction<Scene, LoadSceneMode>;
                        func(scene, mode);
                    }
                }
            };
            UnityAction<Scene> sceneUnloaded = (scene) =>
            {
                if (sceneActions.ContainsKey(scene.name) && sceneActions[scene.name].SceneUnloaded != null)
                {
                    Delegate[] unloaded = sceneActions[scene.name].SceneUnloaded.GetInvocationList();
                    for (int i = 0; i < unloaded.Length; i++)
                    {
                        UnityAction<Scene> func = unloaded[i] as UnityAction<Scene>;
                        func(scene);
                    }
                }
            };

            SceneAction action = new SceneAction();
            action.SceneLoaded += sceneLoaded;
            action.SceneUnloaded += sceneUnloaded;
            sceneActions.Add("SceneManager", action);

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += action.SceneLoaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += action.SceneUnloaded;
        }

        #region Event Handler

        public bool IsContainSceneEventHandler(SceneType scene)
        {
            return sceneActions.ContainsKey(scene.GetDescription());
        }

        public void AddLoadedEventHandler(SceneType scene, UnityAction<Scene, LoadSceneMode> sceneLoaded)
        {
            if(sceneActions.ContainsKey(scene.GetDescription()) == false)
            {
                sceneActions.Add(scene.GetDescription(), new SceneAction());
            }
            sceneActions[scene.GetDescription()].SceneLoaded += sceneLoaded;
        }

        public void RemoveLoadedEventHandler(SceneType scene, UnityAction<Scene, LoadSceneMode> sceneLoaded)
        {
            if(sceneActions.ContainsKey(scene.GetDescription()))
            {
                sceneActions[scene.GetDescription()].SceneLoaded -= sceneLoaded;
            }
        }

        public void AddUnloadedEventHandler(SceneType scene, UnityAction<Scene> sceneUnloaded)
        {
            if (sceneActions.ContainsKey(scene.GetDescription()) == false)
            {
                sceneActions.Add(scene.GetDescription(), new SceneAction());
            }
            sceneActions[scene.GetDescription()].SceneUnloaded += sceneUnloaded;
        }

        public void RemoveUnloadedEventHandler(SceneType scene, UnityAction<Scene> sceneUnloaded)
        {
            if (sceneActions.ContainsKey(scene.GetDescription()))
            {
                sceneActions[scene.GetDescription()].SceneUnloaded -= sceneUnloaded;
            }
        }

        public void ClearLoadedEventHandler(SceneType scene)
        {
            if (sceneActions.ContainsKey(scene.GetDescription()))
            {
                sceneActions[scene.GetDescription()].SceneLoaded = null;
            }
        }

        public void ClearUnloadedEventHandler(SceneType scene)
        {
            if (sceneActions.ContainsKey(scene.GetDescription()))
            {
                sceneActions[scene.GetDescription()].SceneUnloaded = null;
            }
        }

        public void RemoveAllEventHandler(SceneType scene)
        {
            sceneActions.Remove(scene.GetDescription());
        }

        #endregion

        public void LoadScene(SceneType scene, LoadSceneMode loadMode = LoadSceneMode.Single, UnityAction<Scene, LoadSceneMode> sceneLoaded = null)
        {
            if(scene == currentScene)
                return;

            if(sceneLoaded != null)
            {
                AddLoadedEventHandler(scene, sceneLoaded);
            }

            NextScene = scene;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene.GetDescription(), loadMode);
        }
    }
}