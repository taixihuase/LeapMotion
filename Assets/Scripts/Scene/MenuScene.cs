using Core.Manager;
using UnityEngine;

namespace Scene
{
    public class MenuScene : MonoBehaviour
    {
        void Start()
        {
            if (CameraManager.Instance.IsChanging == false)
            {
                CameraManager.Instance.ChangeScene(0, 0.5f, 0.5f, () =>
                {
                    UIManager.Instance.OpenWindow(Define.SceneType.MenuScene, Define.WindowType.Menu, null, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
                });
            }
            else
            {
                UIManager.Instance.OpenWindow(Define.SceneType.MenuScene, Define.WindowType.Menu, null, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
            }
        }
    }
}
