using Controller;
using Core.MVC;
using UnityEngine;
using DG.Tweening;
using Core.Manager;

namespace View.Bathroom
{
    public class BathroomView : SceneEntityView
    {
        void Start()
        {
            Init(BathroomCtrl.Instance.Model);
            Bind(Define.EventType.MoveCameraPos, MoveCamera);
            Bind(Define.EventType.WaterInToggleChanged, OnInToggleStateChanged);
            Bind(Define.EventType.WaterOutToggleChanged, OnOutToggleStateChanged);
            smoke = smokeSystem[(int)GlobalManager.Instance.SceneMode];
            smoke.gameObject.SetActive(true);
            wt = water[(int)GlobalManager.Instance.SceneMode];
            wt.gameObject.SetActive(true);
        }

        [SerializeField]
        Transform waterInToggle;

        [SerializeField]
        float inToggleUpDownDistance;

        bool isInDown = false;

        [SerializeField]
        Transform waterOutToggle;

        [SerializeField]
        float outToggleUpDownDistance;

        bool isOutDown = false;

        [SerializeField]
        float waterInSpeed;

        [SerializeField]
        float waterOutSpeed;

        [SerializeField]
        float waterMaxHeight;

        [SerializeField]
        float waterMinHeight;

        [SerializeField]
        Transform[] water;

        [SerializeField]
        GameObject[] waterParticle;

        [SerializeField]
        ParticleSystem[] smokeSystem;

        float interval = 0.5f;

        float cdTimer = 0;

        bool isAnyStateChanged = false;

        [SerializeField]
        GameObject normalLight;

        [SerializeField]
        GameObject greenLight;

        ParticleSystem smoke;

        Transform wt;

        void Update()
        {
            if(isAnyStateChanged)
            {
                if(cdTimer < interval)
                {
                    cdTimer += Time.deltaTime;
                }
                else
                {
                    cdTimer = 0;
                    isAnyStateChanged = false;
                }
            }

            if(isInDown && wt.localPosition.y < waterMaxHeight)
            {
                wt.Translate(Vector3.up * waterInSpeed);
            }
            if(isOutDown && wt.localPosition.y > waterMinHeight)
            {
                wt.Translate(Vector3.down * waterOutSpeed);
            }

            if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.ThrillingMode)
            {
                if (wt.localPosition.y < waterMinHeight * 0.95f)
                {
                    if (greenLight.activeSelf)
                    {
                        normalLight.SetActive(true);
                        greenLight.SetActive(false);
                        BathroomCtrl.Instance.PourWater();
                    }
                }
                else
                {
                    if (!greenLight.activeSelf)
                    {
                        normalLight.SetActive(false);
                        greenLight.SetActive(true);
                        BathroomCtrl.Instance.FillWater();
                    }
                }
            }

            if (wt.localPosition.y < (waterMaxHeight - waterMinHeight) * 0.35f + waterMinHeight)
            {
                if(smoke.isPlaying)
                {
                    smoke.Stop();
                }
            }
            else
            {
                if (smoke.isStopped)
                {
                    smoke.Play();
                }
            }
        }

        private void OnInToggleStateChanged(params object[] arg1)
        {
            if (isAnyStateChanged)
                return;

            isAnyStateChanged = true;
            if (isInDown == false)
                OnInToggleDown();
            else
                OnInToggleUp();
        }

        private void OnOutToggleStateChanged(params object[] arg1)
        {
            if (isAnyStateChanged)
                return;

            isAnyStateChanged = true;
            if (isOutDown == false)
                OnOutToggleDown();
            else
                OnOutToggleUp();
        }

        private void OnInToggleDown()
        {
            isInDown = true;
            waterParticle[(int)GlobalManager.Instance.SceneMode].SetActive(true);
            waterInToggle.DOLocalMoveZ(waterInToggle.localPosition.z - inToggleUpDownDistance, 0.2f)
                .OnPlay(() => BathroomCtrl.Instance.FillWaterBegin());
            PlayEffectSounds("WaterIn");
        }

        private void OnInToggleUp()
        {
            isInDown = false;
            waterParticle[(int)GlobalManager.Instance.SceneMode].SetActive(false);
            waterInToggle.DOLocalMoveZ(waterInToggle.localPosition.z + inToggleUpDownDistance, 0.2f);
            StopEffectSounds("WaterIn");
        }

        private void OnOutToggleDown()
        {
            isOutDown = true;
            waterOutToggle.DOLocalMoveY(waterOutToggle.localPosition.y - outToggleUpDownDistance, 0.2f)
                .OnPlay(() => BathroomCtrl.Instance.PourWaterBegin());
        }

        private void OnOutToggleUp()
        {
            isOutDown = false;
            waterOutToggle.DOLocalMoveY(waterOutToggle.localPosition.y + outToggleUpDownDistance, 0.2f);           
        }
    }
}
