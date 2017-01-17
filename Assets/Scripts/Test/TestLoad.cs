using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Test
{
    public class TestLoad : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.Log(Core.Manager.SceneManager.Instance.CurrentSceneName + " loaded");
            Core.Manager.CoroutineManager.Instance.StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            UnityAction<Scene> unloaded = (sc) =>
            {
                Debug.Log(sc.name + " unloaded");
            };

            Core.Manager.SceneManager.Instance.AddUnloadedEventHandler(Define.SceneType.TestLoad, unloaded);
            yield return new WaitForSecondsRealtime(5f);

            Core.Manager.SceneManager.Instance.LoadScene(Define.SceneType.TestScene, LoadSceneMode.Single, (sc, mode) =>
            {
                Debug.Log(sc.name + " loaded");
                if(Core.Manager.SceneManager.Instance.IsContainSceneEventHandler(Define.SceneType.TestLoad))
                {
                    Core.Manager.SceneManager.Instance.RemoveUnloadedEventHandler(Define.SceneType.TestLoad, unloaded);
                }
            });
        }
    }
}
