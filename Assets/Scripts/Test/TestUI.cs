using UnityEngine;
using Core.Manager;
using Define;
using System;
using System.Collections;

namespace Test
{
    public class TestUI : MonoBehaviour
    {
        private void Start()
        {
            UIManager.Instance.OpenWindow(SceneManager.Instance.CurrentScene, WindowType.WinTest, (go) =>
            {
                Debug.Log(go.name);
            }, true, true);

            CoroutineManager.Instance.StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(5f);

            SceneManager.Instance.LoadSceneAsync(SceneType.TestLoad);
        }
    }
}
