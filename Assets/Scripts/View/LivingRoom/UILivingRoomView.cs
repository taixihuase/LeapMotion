using Controller;
using Core.Manager;
using Core.MVC;
using Define;
using Model;
using Tool;
using UnityEngine;
using UnityEngine.SceneManagement;
using View.Hallway;
using View.LivingRoom;
using EventType = Define.EventType;
using SceneManager = Core.Manager.SceneManager;

namespace View.Living
{
    public class UILivingRoomView : UIView
    {
        private void Start()
        {
            Init(LivingRoomCtrl.Instance.Model);
            Bind(EventType.InsertPlugComplete, OnInsertPlug);
            Bind(EventType.PutPlugOut, OnPutPlugOut);
            if(GlobalManager.Instance.SceneMode == GlobalManager.Mode.PracticeMode)
            {
                LivingRoomModel m = model as LivingRoomModel;
                clickTips.SetActive(m.CanShowClickTips);
                socketTips.SetActive(m.CanShowSocketTips);
                warningTips.SetActive(false);

                Bind(EventType.ElectricWarning, ShowWarning);
                Bind(EventType.CancelElectricWarning, CancelWarning);
            }
            else
            {
                clickTips.SetActive(false);
                socketTips.SetActive(false);
                warningTips.SetActive(false);
            }
        }

        private void OnInsertPlug(params object[] arg1)
        {
            (model as LivingRoomModel).SetSocketTips(false);
            socketTips.SetActive(false);
            ChangeGreenUIColor();
            pos[3].SetActive(false);
            LivingRoomCtrl.Instance.MovePos(2, () =>
            {
                pos[2].SetActive(true);
            });
        }

        private void OnPutPlugOut(params object[] arg1)
        {
            ChangeNormalUIColor();
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
            (model as LivingRoomModel).SetClickTips(false);
            clickTips.SetActive(false);
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
            LivingRoomCtrl.Instance.FixPlugPosition();
        }

        public void OnClickToHallway()
        {
            LivingRoomCtrl.Instance.PutPlugOut();
            GameObject livingroom = FindObjectOfType<LivingRoomView>().gameObject;
            Object res = ResourceManager.Instance.GetResource(ResourceType.Scene, "Hallway");
            if (res != null)
            {
                UIManager.Instance.CloseWindow(SceneType.MainScene, WindowType.LivingRoom);
                CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
                {
                    DestroyImmediate(livingroom);
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
                    UIManager.Instance.OpenWindow(SceneType.MainScene, WindowType.Hallway, null, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
                });
            }
        }

        public void OnClickExit()
        {
            LivingRoomCtrl.Instance.PutPlugOut();
            CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
            {
                GameObject livingroom = FindObjectOfType<LivingRoomView>().gameObject;
                DestroyImmediate(livingroom);
                if (ResourceManager.Instance.IsResLoaded(ResourceType.Scene, null))
                {
                    Object res = ResourceManager.Instance.GetResource(ResourceType.Scene, null);
                    if (res is AssetBundle)
                    {
                        string path = PathHelper.Instance.GetAssetBundlePath(ResourceType.Scene);
                        ResourceManager.Instance.RemoveLoadedAsset(path);
                        (res as AssetBundle).Unload(true);
                    }
                }
                UIManager.Instance.CloseSceneWindows(SceneType.MainScene);
                SceneManager.Instance.LoadSceneAsync(SceneType.MenuScene, LoadSceneMode.Single, null);
            });
        }

        [SerializeField]
        GameObject clickTips;

        [SerializeField]
        GameObject socketTips;

        [SerializeField]
        GameObject warningTips;

        bool isDelayCancelWarning;

        float delayCancelWarningTime;

        bool isWarning;

        private void ShowWarning(params object[] arg1)
        {
            isDelayCancelWarning = false;
            warningTips.SetActive(true);
            isWarning = true;
        }

        private void CancelWarning(params object[] arg1)
        {
            isDelayCancelWarning = true;
            delayCancelWarningTime = 0.5f;
            isWarning = false;
        }

        protected override void Update()
        {
            base.Update();
            if(isDelayCancelWarning)
            {
                if(delayCancelWarningTime <= 0)
                {
                    warningTips.SetActive(false);
                    isDelayCancelWarning = false;
                }
                else
                {
                    delayCancelWarningTime -= Time.deltaTime;
                }
            }
        }

        public void WarningSound()
        {
            if (isWarning == false)
            {
                LivingRoomCtrl.Instance.ElectricWarning();
            }
            else
            {
                LivingRoomCtrl.Instance.CancelWarning();
            }
        }
    }
}