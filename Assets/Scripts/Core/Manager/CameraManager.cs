using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace Core.Manager
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        public Camera Camera;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            Camera = Camera.main;
            blackBG = GameObject.Find("BlackBG").GetComponent<Image>();
        }

        private bool isStart = false;

        private Vector3 toPos;

        private Quaternion toRot;

        private float smoothTime = 0.3f;

        private Vector3 velocity = Vector3.zero;

        private void Update()
        {
            if (isStart)
            {
                Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, toPos, ref velocity, smoothTime);
                Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, toRot, 1 / smoothTime * Time.deltaTime);
                if (IsArriveTargetPos(Camera.transform.position, toPos) && IsArriveTargetRot(Camera.transform.rotation, toRot))
                {
                    isStart = false;
                }
            }
        }

        private bool IsArriveTargetPos(Vector3 current, Vector3 target)
        {
            return Vector3.SqrMagnitude(current - target) < Vector3.kEpsilon;
        }

        private bool IsArriveTargetRot(Quaternion current, Quaternion target)
        {
            return IsArriveTargetPos(current.eulerAngles, target.eulerAngles);
        }

        public void MoveAndRotate(Transform target)
        {
            isStart = false;
            toPos = target.position;
            toRot = target.rotation;
            isStart = true;
        }

        [SerializeField]
        Image blackBG;

        public void ChangeScene(float darkDuration, float pauseDuration, float showDuration, Action onComplete)
        {
            ViewToDark(darkDuration)
                .OnComplete(() => ViewToDark(pauseDuration)
                .OnComplete(() =>
                {
                    ViewToDark(pauseDuration)
                    .OnPlay(() => onComplete())
                    .OnComplete(() => ShowView(showDuration));
                }));
        }
    
        public Tweener ViewToDark(float duration)
        {
            return blackBG.DOFade(1, duration);
        }

        public Tweener ShowView(float duration)
        {
            return blackBG.DOFade(0, duration);
        }
    }
}
