using Controller;
using Core.Manager;
using Core.MVC;
using Model;
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

            if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.PracticeMode)
            {
                BathroomModel m = model as BathroomModel;
                pourTips.SetActive(false);
                fillTips.SetActive(m.CanShowFillTips);

                Bind(Define.EventType.FillWaterBegin, FillWaterBegin);
                Bind(Define.EventType.PourWaterBegin, PourWaterBegin);
            }
            else
            {
                pourTips.SetActive(false);
                fillTips.SetActive(false);
            }
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
            Object res = ResourceManager.Instance.GetResource(Define.ResourceType.Scene, "Hallway");
            if (res != null)
            {
                UIManager.Instance.CloseWindow(Define.SceneType.MainScene, Define.WindowType.Bathroom);
                CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
                {
                    Destroy(bathroom);
                    GameObject obj;
                    if (res is AssetBundle)
                    {
                        obj = Instantiate((res as AssetBundle).LoadAsset("Hallway")) as GameObject;
                    }
                    else
                    {
                        obj = Instantiate(res) as GameObject;
                    }
                    Transform startPos = obj.GetComponent<HallwayView>().GetStartPos();
                    CameraManager.Instance.MoveAndRotate(startPos);
                    UIManager.Instance.OpenWindow(Define.SceneType.MainScene, Define.WindowType.Hallway, null, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
                });
            }
        }

        [SerializeField]
        GameObject pourTips;

        [SerializeField]
        GameObject fillTips;

        private void PourWaterBegin(params object[] arg1)
        {
            BathroomModel bm = model as BathroomModel;
            if (bm.CanShowFillTips == false)
            {
                bm.SetPourTips(false);
                pourTips.SetActive(false);
            }
        }

        private void FillWaterBegin(params object[] arg1)
        {
            BathroomModel bm = model as BathroomModel;
            bm.SetFillTips(false);
            fillTips.SetActive(false);
            pourTips.SetActive(bm.CanShowPourTips);
        }

        public void ChangeWaterIn()
        {
            BathroomCtrl.Instance.ChangeWaterInToggle();
        }
    }
}