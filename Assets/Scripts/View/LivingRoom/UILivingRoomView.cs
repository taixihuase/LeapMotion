using Controller;
using Core.Manager;
using UnityEngine;
using View.Hallway;
using View.LivingRoom;

namespace View.Living
{
    public class UILivingRoomView : Core.MVC.View
    {
        [SerializeField]
        GameObject[] pos;

        void Start()
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (i != 0)
                    pos[i].SetActive(false);
            }
        }

        public void OnClickToPos1()
        {
            pos[1].SetActive(false);
            LivingRoomCtrl.Instance.MovePos(0, () =>
            {
                pos[0].SetActive(true);
            });
        }

        public void OnClickToPos2()
        {
            pos[0].SetActive(false);
            pos[2].SetActive(false);
            LivingRoomCtrl.Instance.MovePos(1, () => pos[1].SetActive(true));
        }

        public void OnClickToPos3()
        {
            pos[1].SetActive(false);
            pos[3].SetActive(false);
            LivingRoomCtrl.Instance.MovePos(2, () => pos[2].SetActive(true));
        }

        public void OnClickToPos4()
        {
            pos[2].SetActive(false);
            LivingRoomCtrl.Instance.MovePos(3, () => pos[3].SetActive(true));
        }

        public void OnClickToHallway()
        {
            GameObject livingroom = FindObjectOfType<LivingRoomView>().gameObject;
            Object hallway = ResourceManager.Instance.GetResource(Define.ResourceType.Scene, "Hallway");
            if (hallway != null)
            {
                UIManager.Instance.CloseWindow(Define.SceneType.MainScene, Define.WindowType.LivingRoom);
                CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
                {
                    Destroy(livingroom);
                    GameObject obj = Instantiate(hallway) as GameObject;
                    Transform startPos = obj.GetComponent<HallwayView>().GetStartPos();
                    CameraManager.Instance.MoveAndRotate(startPos);
                    UIManager.Instance.OpenWindow(Define.SceneType.MainScene, Define.WindowType.Hallway);
                });
            }
        }
    }
}