using Controller;
using Core.MVC;
using Leap;
using Leap.Unity.Interaction;
using System;
using Core.Manager;
using Leap.Unity;
using UnityEngine;

namespace View.LivingRoom
{
    public class LivingRoomView : SceneEntityView
    {
        void Start()
        {
            Init(LivingRoomCtrl.Instance.Model);
            Bind(Define.EventType.MoveCameraPos, MoveCamera);
            Bind(Define.EventType.InsertPlug, InsertPlug);
            Bind(Define.EventType.PutPlugOut, PutPlugOut);
            Bind(Define.EventType.FixPlugPos, FixPlugPos);
            if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.PracticeMode)
            {
                Bind(Define.EventType.ElectricWarning, ShowWarning);
                Bind(Define.EventType.CancelElectricWarning, CancelWarning);
            }
            else
            {
                PlayEnvironmentSounds("LivingRoomT");
            }

            lightStartColor = dirLight.color;
            lightStartIntensity = dirLight.intensity;
            if (plugInteraction.Manager == null)
            {
                plugInteraction.Manager = LeapMotionManager.Instance.Interaction;
            }

            insertFunc = (h) =>
            {
                isInsert = true;
                timer = 0;
                plugInteraction.isKinematic = true;
                plugInteraction.useGravity = false;
                plug.transform.localPosition = insertPos.transform.localPosition;
                plug.transform.localRotation = insertPos.transform.localRotation;
                if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.ThrillingMode)
                {
                    dirLight.color = lightChangeColor;
                    dirLight.intensity = lightChangeIntensity;
                    greenLight.gameObject.SetActive(true);
                }
                LivingRoomCtrl.Instance.OnInsertPlugComplete();
            };
        }

        [SerializeField]
        Transform insertPos;

        [SerializeField]
        Transform plugStartPos;

        [SerializeField]
        Transform plug;

        [SerializeField]
        InteractionBehaviour plugInteraction;

        [SerializeField]
        Light dirLight;

        [SerializeField]
        Light greenLight;

        Color lightStartColor;

        [SerializeField]
        Color lightChangeColor;

        float lightStartIntensity;

        [SerializeField]
        float lightChangeIntensity;

        Action<Hand> insertFunc = null;

        bool isInsert = false;

        float duration = 1f;

        float timer = 0;

        float fixDuration = 1f;

        float fixTimer = 1f;

        private void InsertPlug(params object[] arg1)
        {
            plugInteraction.OnHandReleasedEvent += insertFunc;
            if ((LeapMotionManager.Instance.Provider as LeapServiceProvider).IsConnected() == false)
            {
                insertFunc.Invoke(null);
            }
        }

        private void PutPlugOut(params object[] arg1)
        {
            if (insertFunc != null)
            {
                isInsert = false;
                plugInteraction.isKinematic = false;
                plugInteraction.useGravity = true;
                plugInteraction.OnHandReleasedEvent -= insertFunc;
                dirLight.color = lightStartColor;
                dirLight.intensity = lightStartIntensity;
                greenLight.gameObject.SetActive(false);
            }
        }

        private void FixPlugPos(params object[] arg1)
        {
            plugInteraction.isKinematic = true;
            fixTimer = 0;
            if(isInsert == false)
            {
                plug.transform.localPosition = plugStartPos.transform.localPosition;
                plug.transform.localRotation = plugStartPos.transform.localRotation;
            }
        }

        private void CancelWarning(object[] args)
        {
            StopEffectSounds("ElectricWarning");
        }

        private void ShowWarning(object[] args)
        {
            PlayEffectSounds("ElectricWarning");
        }

        void Update()
        {
            if (isInsert && timer < duration)
            {
                timer += Time.deltaTime;
                plug.transform.localPosition = insertPos.transform.localPosition;
                plug.transform.localRotation = insertPos.transform.localRotation;
            }
            else if (fixTimer < fixDuration)
            {
                fixTimer += Time.deltaTime;
            }
            else
            {
                if (plugInteraction.isKinematic)
                {
                    plugInteraction.isKinematic = false;
                }
            }
        }
    }
}
