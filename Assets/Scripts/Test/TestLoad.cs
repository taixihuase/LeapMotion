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
            Core.Manager.CoroutineManager.Instance.StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            yield return null;

            UnityAction<Scene> unloaded = (sc) =>
            {
                Debug.Log(sc.name + " unloaded");
            };
            Core.Manager.SceneManager.Instance.AddUnloadedEventHandler(Define.SceneType.TestLoad, unloaded);

            UnityAction<Scene, Scene> changed = (sc1, sc2) =>
            {
                Debug.Log("Change to " + sc2.name);
            };
            Core.Manager.SceneManager.Instance.AddChangedEventHandler(Define.SceneType.TestScene, changed);

            yield return new WaitForSeconds(5f);

            Core.Manager.SceneManager.Instance.LoadSceneAsync(Define.SceneType.TestScene, LoadSceneMode.Additive, (sc, mode) =>
            {
                Debug.Log(Core.Manager.SceneManager.Instance.CurrentSceneName + " loaded");
                Debug.Log(SceneManager.GetActiveScene().name + " is active");
            });
        }
    }
}
