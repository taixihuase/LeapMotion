using Controller;
using Core.Manager;
using Core.MVC;
using UnityEngine;
using View.Hallway;

namespace View.Kitchen
{
    public class UIKitchenView : UIView
    {
        void Start()
        {
            for(int i = 0; i < pos.Length; i++)
            {
                if (i != 0)
                    pos[i].SetActive(false);
            }
        }

        public void OnClickToPos1()
        {
            pos[1].SetActive(false);
            KitchenCtrl.Instance.MovePos(0, () => pos[0].SetActive(true));
        }

        public void OnClickToPos2()
        {
            pos[0].SetActive(false);
            pos[2].SetActive(false);
            KitchenCtrl.Instance.MovePos(1, () => pos[1].SetActive(true));
        }

        public void OnClickToPos3()
        {
            pos[1].SetActive(false);
            KitchenCtrl.Instance.MovePos(2, () => pos[2].SetActive(true));
        }

        public void OnClickToHallway()
        {
            GameObject kitchen = FindObjectOfType<KitchenView>().gameObject;
            Object hallway = ResourceManager.Instance.GetResource(Define.ResourceType.Scene, "Hallway");
            if (hallway != null)
            {
                UIManager.Instance.CloseWindow(Define.SceneType.MainScene, Define.WindowType.Kitchen);
                CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
                {
                    Destroy(kitchen);
                    GameObject obj = Instantiate(hallway) as GameObject;
                    Transform startPos = obj.GetComponent<HallwayView>().GetStartPos();
                    CameraManager.Instance.MoveAndRotate(startPos);
                    UIManager.Instance.OpenWindow(Define.SceneType.MainScene, Define.WindowType.Hallway);
                });
            }
        }
    }
}
