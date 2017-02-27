using UnityEngine;

namespace Scene
{
    public class StartScene : MonoBehaviour
    {
        void Start()
        {
            Core.Manager.SceneManager.Instance.LoadScene(Define.SceneType.MenuScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
