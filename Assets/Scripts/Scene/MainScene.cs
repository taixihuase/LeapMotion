using Controller;
using Core.Manager;
using UnityEngine;
using View.LivingRoom;

namespace Scene
{
    public class MainScene : MonoBehaviour
    {
        void Start()
        {
            ResourceManager.Instance.LoadAsset(Define.ResourceType.Scene, "LivingRoom", (o) =>
            {
                GameObject livingroom;
                if (o is AssetBundle)
                {
                    livingroom = Instantiate((o as AssetBundle).LoadAsset("LivingRoom")) as GameObject;
                }
                else
                {
                    livingroom = Instantiate(o) as GameObject;
                }
                livingroom.SetActive(true);
                LivingRoomView view = livingroom.GetComponent<LivingRoomView>();
                Transform startPos = view.GetStartPos();
                Camera camera = CameraManager.Instance.Camera;
                camera.transform.position = startPos.position;
                camera.transform.rotation = startPos.rotation;

                LivingRoomCtrl.Instance.Model.Reset();
                BathroomCtrl.Instance.Model.Reset();
                KitchenCtrl.Instance.Model.Reset();

                UIManager.Instance.OpenWindow(Define.SceneType.MainScene, Define.WindowType.LivingRoom, null,
                    ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
            }, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);

            ResourceManager.Instance.LoadAsset(Define.ResourceType.Scene, "Bathroom", null,
                ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);

            ResourceManager.Instance.LoadAsset(Define.ResourceType.Scene, "Kitchen", null,
                ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);

            ResourceManager.Instance.LoadAsset(Define.ResourceType.Scene, "Hallway", null,
                ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
        }
    }
}
