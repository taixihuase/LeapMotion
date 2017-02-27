using Core.Manager;
using UnityEngine;

namespace Scene
{
    public class MenuScene : MonoBehaviour
    {
        void Start()
        {
            CameraManager.Instance.ChangeScene(0, 1.0f, 0.5f, () =>
            {
                UIManager.Instance.OpenWindow(Define.SceneType.MenuScene, Define.WindowType.Menu, null, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
            });
        }
    }
}
