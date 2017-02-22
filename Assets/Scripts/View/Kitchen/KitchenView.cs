using Controller;
using Core.MVC;
using UnityEngine;
using DG.Tweening;

namespace View.Kitchen
{
    public class KitchenView : EntityView
    {
        void Start()
        {
            Init(KitchenCtrl.Instance.Model);
            Bind(Define.EventType.MoveCameraPos, MoveCamera);
            Bind(Define.EventType.FireSwitchChanged, OnFireUsingStateChanged);
            Bind(Define.EventType.FridgeDoorChanged, OnFridgeDoorStateChange);
            boxStartPos = bottomFridgeBox.localPosition;
        }

        [SerializeField]
        GameObject light;

        [SerializeField]
        Transform[] fireSwitch;

        float[] interval = new float[] { 0.5f, 0.5f };

        float[] cdTimer = new float[] { 0, 0 };

        bool[] isFireStateChanged = new bool[] { false, false };

        bool[] isUsingFire = new bool[] { false, false };

        [SerializeField]
        GameObject[] fire;

        [SerializeField]
        float fireSwitchMoveDistance;

        [SerializeField]
        Transform[] fridgeDoor;

        [SerializeField]
        Transform bottomFridgeBox;

        Vector3 boxStartPos;

        [SerializeField]
        Transform boxFinalPos;

        [SerializeField]
        GameObject headFire;

        bool isFridgeDoorOpened = false;
                    
        void Update()
        {
            for(int i = 0; i < fireSwitch.Length; i++)
            {
                if(isFireStateChanged[i])
                {
                    if (cdTimer[i] < interval[i])
                    {
                        cdTimer[i] += Time.deltaTime;
                    }
                    else
                    {
                        cdTimer[i] = 0;
                        isFireStateChanged[i] = false;
                    }
                }
            }
        }        
         
        private void OnFireUsingStateChanged(params object[] arg1)
        {
            int index = (int)arg1[0];
            if (isFireStateChanged[index])
                return;

            isFireStateChanged[index] = true;
            if (isUsingFire[index] == false)
                OnOpenFire(index);
            else
                OnCloseFire(index);

        }

        private void OnOpenFire(int index)
        {
            isUsingFire[index] = true;
            fire[index].SetActive(true);
            fireSwitch[index].DOLocalMoveZ(fireSwitch[index].localPosition.z - fireSwitchMoveDistance, 0.2f);
        }

        private void OnCloseFire(int index)
        {
            isUsingFire[index] = false;
            fire[index].SetActive(false);
            fireSwitch[index].DOLocalMoveZ(fireSwitch[index].localPosition.z + fireSwitchMoveDistance, 0.2f);
        }

        private void OnFridgeDoorStateChange(params object[] arg1)
        {
            int index = (int)arg1[0];
            if (index == 1)
            {
                if (isFridgeDoorOpened == false)
                {
                    bottomFridgeBox.DOKill();
                    bottomFridgeBox.DOLocalMove(boxFinalPos.transform.localPosition, 0.35f)
                        .OnPlay(() => headFire.SetActive(true));
                }
                else
                {
                    bottomFridgeBox.DOKill();
                    bottomFridgeBox.DOLocalMove(boxStartPos, 0.35f)
                        .OnPlay(() => headFire.SetActive(false));
                }
                isFridgeDoorOpened = !isFridgeDoorOpened;
            }
        }
    }
}
