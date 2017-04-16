using Controller;
using Core.MVC;
using UnityEngine;
using DG.Tweening;
using Core.Manager;

namespace View.Kitchen
{
    public class KitchenView : SceneEntityView
    {
        void Start()
        {
            Init(KitchenCtrl.Instance.Model);
            Bind(Define.EventType.MoveCameraPos, MoveCamera);
            Bind(Define.EventType.FireSwitchChanged, OnFireUsingStateChanged);
            Bind(Define.EventType.FridgeDoorChanged, OnFridgeDoorStateChange);
            Bind(Define.EventType.KitchenLightChanged, OnLightStateChange);
            boxStartPos = bottomFridgeBox.localPosition;
            if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.ThrillingMode)
            {
                fridgeBoxContent[0].SetActive(false);
                fridgeBoxContent[1].SetActive(true);
                PlayEnvironmentSounds("KitchenT");
            }
            else
            {
                fridgeBoxContent[0].SetActive(true);
                fridgeBoxContent[1].SetActive(false);
            }
        }

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

        [SerializeField]
        GameObject[] lights;

        [SerializeField]
        GameObject[] fridgeBoxContent;

        bool isLightOn = false;
                    
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
            if (index == 0)
            {
                PlayEffectSounds("OpenFireLeft");
            }
            else
            {
                PlayEffectSounds("OpenFireRight");
            }
        }

        private void OnCloseFire(int index)
        {
            isUsingFire[index] = false;
            fire[index].SetActive(false);
            fireSwitch[index].DOLocalMoveZ(fireSwitch[index].localPosition.z + fireSwitchMoveDistance, 0.2f);
            if (index == 0)
            {
                PlayEffectSounds("CloseFireLeft");
            }
            else
            {
                PlayEffectSounds("CloseFireRight");
            }
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
                    if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.ThrillingMode)
                    {
                        PlayEffectSounds("HeadOut");
                    }
                }
                else
                {
                    bottomFridgeBox.DOKill();
                    bottomFridgeBox.DOLocalMove(boxStartPos, 0.35f)
                        .OnPlay(() => headFire.SetActive(false));
                    if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.ThrillingMode)
                    {
                        PlayEffectSounds("HeadIn");
                    }
                }
                isFridgeDoorOpened = !isFridgeDoorOpened;
            }
        }

        private void OnLightStateChange(params object[] arg1)
        {
            isLightOn = !isLightOn;
            lights[(int)GlobalManager.Instance.SceneMode].SetActive(isLightOn);
            if(isLightOn)
            {
                if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.ThrillingMode)
                {
                    PlayEnvironmentSounds("ElfFireOn");
                    for (int i = 0; i < fire.Length; i++)
                    {
                        ParticleSystem ps = fire[i].transform.GetChild(0).GetComponent<ParticleSystem>();
                        ps.startColor = new Color(0, 150f / 255, 1f, 1f);
                    }
                }
            }
            else
            {
                for (int i = 0; i < fire.Length; i++)
                {
                    ParticleSystem ps = fire[i].transform.GetChild(0).GetComponent<ParticleSystem>();
                    ps.startColor = new Color(225f / 255, 1f, 150f / 255, 1f);
                }
                if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.ThrillingMode)
                {
                    PlayEnvironmentSounds("KitchenT");
                }
                else
                {
                    StopEnvironmentSounds();
                }
            }
        }
    }
}
