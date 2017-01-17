using UnityEngine;
using Core.Manager;
using Define;
using System;
using System.Collections;

namespace Test
{
    public class TestUI : MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance.Init();
        }

        private void Start()
        {
            Action<UnityEngine.Object> act = (obj) =>
            {
                GameObject go = obj as GameObject;
                if(go != null)
                {
                    GameObject instance = Instantiate(go);
                    instance.transform.parent = UIManager.Instance.Root;
                    instance.transform.localPosition = Vector3.zero;
                    instance.transform.localScale = Vector3.one;
                }
            };
            ResourceManager.Instance.LoadAsset(ResourceType.UI, "WinTest", act, true);

            CoroutineManager.Instance.StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            yield return new WaitForSeconds(5f);

            SceneManager.Instance.LoadScene(SceneType.TestLoad);
        }
    }
}
