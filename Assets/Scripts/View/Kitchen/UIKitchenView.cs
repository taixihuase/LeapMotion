using System.Collections.Generic;
using Controller;
using Core.Manager;
using Core.MVC;
using Model;
using UnityEngine;
using View.Hallway;

namespace View.Kitchen
{
    public class UIKitchenView : UIView
    {
        void Start()
        {
            Init(KitchenCtrl.Instance.Model);
            Bind(Define.EventType.KitchenLightChanged, KitchenLightChanged);
            Bind(Define.EventType.FireSwitchChanged, FireChanged);
            Bind(Define.EventType.FridgeDoorChanged, FridgeDoorChanged);

            if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.PracticeMode)
            {
                KitchenModel m = model as KitchenModel;
                lightTips.SetActive(m.CanShowLightTips);
                fireTips.SetActive(m.CanShowFireTips);
                fridgeTips.SetActive(m.CanShowFridgeTips);
            }
            else
            {
                lightTips.SetActive(false);
                fireTips.SetActive(false);
                fridgeTips.SetActive(false);
            }
        }

        bool isNormalColor = true;

        private void KitchenLightChanged(params object[] arg1)
        {
            if(isNormalColor)
            {
                ChangeGreenUIColor();
            }
            else
            {
                ChangeNormalUIColor();
            }
            isNormalColor = !isNormalColor;

            (model as KitchenModel).SetLightTips(false);
            lightTips.SetActive(false);
        }

        private void FireChanged(params object[] arg1)
        {
            (model as KitchenModel).SetFireTips(false);
            fireTips.SetActive(false);
        }

        private void FridgeDoorChanged(params object[] arg1)
        {
            (model as KitchenModel).SetFridgeTips(false);
            fridgeTips.SetActive(false);
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
            Object res = ResourceManager.Instance.GetResource(Define.ResourceType.Scene, "Hallway");
            if (res != null)
            {
                UIManager.Instance.CloseWindow(Define.SceneType.MainScene, Define.WindowType.Kitchen);
                CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
                {
                    DestroyImmediate(kitchen);
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
        GameObject lightTips;

        [SerializeField]
        GameObject fireTips;

        [SerializeField]
        GameObject fridgeTips;

        public void FireLeftSound()
        {
            KitchenCtrl.Instance.ChangeFire(0);
        }

        public void FireRightSound()
        {
            KitchenCtrl.Instance.ChangeFire(1);
        }

        public void FridgeDoorSound()
        {
            KitchenCtrl.Instance.ChangeFridgeDoor(1);
        }

        public void LightSound()
        {
            KitchenCtrl.Instance.ChangeLight();
        }
    }
}
