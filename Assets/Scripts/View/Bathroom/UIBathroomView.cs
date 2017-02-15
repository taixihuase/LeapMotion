using Controller;
using Core.Manager;
using UnityEngine;
using View.Hallway;

namespace View.Bathroom
{
    public class UIBathroomView : Core.MVC.View
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
            BathroomCtrl.Instance.MovePos(0, () => pos[0].SetActive(true));
        }

        public void OnClickToPos2()
        {
            pos[0].SetActive(false);
            BathroomCtrl.Instance.MovePos(1, () => pos[1].SetActive(true));
        }

        public void OnClickToHallway()
        {
            GameObject bathroom = FindObjectOfType<BathroomView>().gameObject;
            Object hallway = ResourceManager.Instance.GetResource(Define.ResourceType.Scene, "Hallway");
            if (hallway != null)
            {
                UIManager.Instance.CloseWindow(Define.SceneType.MainScene, Define.WindowType.Bathroom);
                CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
                {
                    Destroy(bathroom);
                    GameObject obj = Instantiate(hallway) as GameObject;
                    Transform startPos = obj.GetComponent<HallwayView>().GetStartPos();
                    CameraManager.Instance.MoveAndRotate(startPos);
                    UIManager.Instance.OpenWindow(Define.SceneType.MainScene, Define.WindowType.Hallway);
                });
            }
        }
    }

}