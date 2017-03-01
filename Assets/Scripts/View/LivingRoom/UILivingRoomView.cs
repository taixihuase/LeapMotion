using Controller;
using Core.Manager;
using UnityEngine;
using View.Hallway;
using View.LivingRoom;
using Core.MVC;
using Model;

namespace View.Living
{
    public class UILivingRoomView : UIView
    {
        void Start()
        {
            Init(LivingRoomCtrl.Instance.Model);
            Bind(Define.EventType.InsertPlugComplete, OnInsertPlug);
            Bind(Define.EventType.PutPlugOut, OnPutPlugOut);
            if(GlobalManager.Instance.SceneMode == GlobalManager.Mode.PracticeMode)
            {
                LivingRoomModel m = model as LivingRoomModel;
                clickTips.SetActive(m.CanShowClickTips);
                socketTips.SetActive(m.CanShowSocketTips);
                warningTips.SetActive(false);

                Bind(Define.EventType.ElectricWarning, ShowWarning);
                Bind(Define.EventType.CancelElectricWarning, CancelWarning);
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
            LivingRoomCtrl.Instance.MovePos(2, () => pos[2].SetActive(true));
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
                    UIManager.Instance.OpenWindow(Define.SceneType.MainScene, Define.WindowType.Hallway, null, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
                });
            }
        }

        public void OnClickExit()
        {
            CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
            {
                GameObject livingroom = FindObjectOfType<LivingRoomView>().gameObject;
                Destroy(livingroom);
                UIManager.Instance.CloseSceneWindows(Define.SceneType.MainScene);
                SceneManager.Instance.LoadSceneAsync(Define.SceneType.MenuScene, UnityEngine.SceneManagement.LoadSceneMode.Single, null);
            });
        }

        [SerializeField]
        GameObject clickTips;

        [SerializeField]
        GameObject socketTips;

        [SerializeField]
        GameObject warningTips;

        bool isDelayCancelWarning = false;

        float delayCancelWarningTime = 0;

        private void ShowWarning(params object[] arg1)
        {
            isDelayCancelWarning = false;
            warningTips.SetActive(true);
        }

        private void CancelWarning(params object[] arg1)
        {
            isDelayCancelWarning = true;
            delayCancelWarningTime = 0.5f;
        }

        void Update()
        {
            if(isDelayCancelWarning == true)
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
    }
}