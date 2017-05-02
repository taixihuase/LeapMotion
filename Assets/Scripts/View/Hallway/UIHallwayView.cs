using Core.Manager;
using Core.MVC;
using Tool;
using Define;
using UnityEngine;

namespace View.Hallway
{
    public class UIHallwayView : UIView
    {
        public void OnClickToOtherScene(string sceneName)
        {
            GameObject hallway = FindObjectOfType<HallwayView>().gameObject;
            Object res = ResourceManager.Instance.GetResource(ResourceType.Scene, sceneName);
            if (res != null)
            {
                UIManager.Instance.CloseWindow(SceneType.MainScene, WindowType.Hallway);
                CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
                {
                    DestroyImmediate(hallway);
                    GameObject obj;
                    if (res is AssetBundle)
                    {
                        obj = Instantiate((res as AssetBundle).LoadAsset(sceneName)) as GameObject;
                    }
                    else
                    {
                        obj = Instantiate(res) as GameObject;
                    }
                    Transform startPos = obj.GetComponent<SceneEntityView>().GetStartPos();
                    CameraManager.Instance.MoveAndRotate(startPos);
                    UIManager.Instance.OpenWindow(SceneType.MainScene, EnumDescriptionTool.GetEnum<WindowType>(sceneName), null, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
                });
            }
        }
    }
}