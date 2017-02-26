using Controller;
using Core.Manager;
using Core.MVC;
using UnityEngine;
using View.Hallway;

namespace View.Bathroom
{
    public class UIBathroomView : UIView
    {
        void Start()
        {
            Init(BathroomCtrl.Instance.Model);
            Bind(Define.EventType.PourWater, PourWater);
            Bind(Define.EventType.FillWater, FillWater);
        }

        private void PourWater(params object[] arg1)
        {
            ChangeNormalUIColor();
        }

        private void FillWater(params object[] arg1)
        {
            ChangeGreenUIColor();
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
                    UIManager.Instance.OpenWindow(Define.SceneType.MainScene, Define.WindowType.Hallway, null, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
                });
            }
        }
    }

}