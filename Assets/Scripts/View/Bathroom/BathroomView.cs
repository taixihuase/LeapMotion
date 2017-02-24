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
        }

        [SerializeField]
        Transform WaterInToggle;

        [SerializeField]
        float InToggleUpDownDistance;

        bool isInDown = false;

        [SerializeField]
        Transform WaterOutToggle;

        [SerializeField]
        float OutToggleUpDownDistance;

        bool isOutDown = false;

        [SerializeField]
        float WaterInSpeed;

        [SerializeField]
        float WaterOutSpeed;

        [SerializeField]
        float WaterMaxHeight;

        [SerializeField]
        float WaterMinHeight;

        [SerializeField]
        Transform Water;

        [SerializeField]
        GameObject WaterParticle;

        float interval = 0.5f;

        float cdTimer = 0;

        bool isAnyStateChanged = false;

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

            if(isInDown && Water.localPosition.y < WaterMaxHeight)
            {
                Water.Translate(Vector3.up * WaterInSpeed);
            }
            if(isOutDown && Water.localPosition.y > WaterMinHeight)
            {
                Water.Translate(Vector3.down * WaterOutSpeed);
            }
            if(Water.localPosition.y < WaterMinHeight * 0.95f)
            {
                if (greenLight.activeSelf == true)
                {
                    greenLight.SetActive(false);
                    BathroomCtrl.Instance.PourWater();
                }
            }
            else
            {
                if (greenLight.activeSelf == false)
                {
                    greenLight.SetActive(true);
                    BathroomCtrl.Instance.FillWater();
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
            WaterParticle.SetActive(true);
            WaterInToggle.DOLocalMoveZ(WaterInToggle.localPosition.z - InToggleUpDownDistance, 0.2f);
        }

        private void OnInToggleUp()
        {
            isInDown = false;
            WaterParticle.SetActive(false);
            WaterInToggle.DOLocalMoveZ(WaterInToggle.localPosition.z + InToggleUpDownDistance, 0.2f);
        }

        private void OnOutToggleDown()
        {
            isOutDown = true;
            WaterOutToggle.DOLocalMoveY(WaterOutToggle.localPosition.y - OutToggleUpDownDistance, 0.2f);
        }

        private void OnOutToggleUp()
        {
            isOutDown = false;
            WaterOutToggle.DOLocalMoveY(WaterOutToggle.localPosition.y + OutToggleUpDownDistance, 0.2f);
        }
    }
}
