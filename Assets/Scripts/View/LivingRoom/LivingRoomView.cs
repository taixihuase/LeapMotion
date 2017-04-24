using System;
using Controller;
using Core.Manager;
using Core.MVC;
using Define;
using Leap;
using Leap.Unity.Interaction;
using UnityEngine;
using EventType = Define.EventType;

namespace View.LivingRoom
{
    public class LivingRoomView : SceneEntityView
    {
        void Start()
        {
            Init(LivingRoomCtrl.Instance.Model);
            Bind(EventType.MoveCameraPos, MoveCamera);
            Bind(EventType.InsertPlug, InsertPlug);
            Bind(EventType.PutPlugOut, PutPlugOut);
            Bind(EventType.FixPlugPos, FixPlugPos);
            if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.PracticeMode)
            {
                Bind(EventType.ElectricWarning, ShowWarning);
                Bind(EventType.CancelElectricWarning, CancelWarning);
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

            insertFunc = h =>
            {
                isInsert = true;
                timer = 0;
                plugInteraction.isKinematic = true;
                plugInteraction.useGravity = false;
                plug.transform.localPosition = insertPos.transform.localPosition;
                plug.transform.localRotation = insertPos.transform.localRotation;
                plugView.IsDrag = false;
                if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.ThrillingMode)
                {
                    dirLight.color = lightChangeColor;
                    dirLight.intensity = lightChangeIntensity;
                    greenLight.gameObject.SetActive(true);
                }

                string videoName = "Normal";
                if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.ThrillingMode)
                {
                    videoName = "Thrilling";
                }
                ResourceManager.Instance.LoadAsset(ResourceType.Video, videoName, o =>
                {
                    video.gameObject.SetActive(true);
                    MovieTexture mt;
                    if (o is AssetBundle)
                    {
                        mt = (o as AssetBundle).LoadAsset(videoName) as MovieTexture;
                    }
                    else
                    {
                        mt = o as MovieTexture;
                    }
                    if (mt != null)
                    {
                        mt.loop = true;
                        video.material.mainTexture = mt;
                        mt.Play();
                        ResourceManager.Instance.RegistResource(ResourceType.Sound, "Normal", mt.audioClip);
                        SoundManager.Instance.PlayEffectSound("Normal", true, 1f, 2);
                    }
                }, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);

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
        PlugView plugView;

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

        Action<Hand> insertFunc;

        bool isInsert;

        float duration = 1f;

        float timer;

        float fixDuration = 1f;

        float fixTimer = 1f;

        [SerializeField]
        MeshRenderer video;

        private void InsertPlug(params object[] arg1)
        {
            if (plugView.IsDrag == false)
            {
                plugInteraction.OnHandReleasedEvent += insertFunc;
            }
            else
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

                MovieTexture mt = video.material.mainTexture as MovieTexture;
                if (mt != null)
                {
                    mt.Stop();
                    video.material.mainTexture = null;
                    SoundManager.Instance.StopEffectSound(2);
                }
                video.gameObject.SetActive(false);
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
