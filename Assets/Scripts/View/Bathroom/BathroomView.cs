using Controller;
using Core.MVC;
using UnityEngine;
using DG.Tweening;

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
            smokeSystem.Play();
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
        Transform water;

        [SerializeField]
        GameObject waterParticle;

        [SerializeField]
        ParticleSystem smokeSystem;

        float interval = 0.5f;

        float cdTimer = 0;

        bool isAnyStateChanged = false;

        [SerializeField]
        GameObject normalLight;

        [SerializeField]
        GameObject greenLight;

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

            if(isInDown && water.localPosition.y < waterMaxHeight)
            {
                water.Translate(Vector3.up * waterInSpeed);
            }
            if(isOutDown && water.localPosition.y > waterMinHeight)
            {
                water.Translate(Vector3.down * waterOutSpeed);
            }
            if(water.localPosition.y < waterMinHeight * 0.95f)
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
                    smokeSystem.Play();
                    BathroomCtrl.Instance.FillWater();
                }
            }
            if (water.localPosition.y < (waterMaxHeight - waterMinHeight) * 0.35f + waterMinHeight)
            {
                if(smokeSystem.isPlaying)
                {
                    smokeSystem.Stop();
                }
            }
            else
            {
                if (smokeSystem.isStopped)
                {
                    smokeSystem.Play();
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
            waterParticle.SetActive(true);
            waterInToggle.DOLocalMoveZ(waterInToggle.localPosition.z - inToggleUpDownDistance, 0.2f);
        }

        private void OnInToggleUp()
        {
            isInDown = false;
            waterParticle.SetActive(false);
            waterInToggle.DOLocalMoveZ(waterInToggle.localPosition.z + inToggleUpDownDistance, 0.2f);
        }

        private void OnOutToggleDown()
        {
            isOutDown = true;
            waterOutToggle.DOLocalMoveY(waterOutToggle.localPosition.y - outToggleUpDownDistance, 0.2f);
        }

        private void OnOutToggleUp()
        {
            isOutDown = false;
            waterOutToggle.DOLocalMoveY(waterOutToggle.localPosition.y + outToggleUpDownDistance, 0.2f);
        }
    }
}
