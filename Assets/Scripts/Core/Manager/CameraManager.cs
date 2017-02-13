﻿using UnityEngine;

namespace Core.Manager
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        private Camera camera;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            camera = Camera.main;
        }

        private bool isStart = false;

        private Vector3 toPos;

        private Quaternion toRot;

        private float smoothTime = 0.5f;

        private Vector3 velocity = Vector3.zero;

        private void Update()
        {
            if (isStart)
            {
                camera.transform.position = Vector3.SmoothDamp(camera.transform.position, toPos, ref velocity, smoothTime);
                camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, toRot, 1 / smoothTime * Time.deltaTime);
                if (IsArriveTargetPos(camera.transform.position, toPos) && IsArriveTargetRot(camera.transform.rotation, toRot))
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
            return Vector3.SqrMagnitude(Quaternion.Inverse(current) * target.eulerAngles) < Vector3.kEpsilon;
        }

        public void MoveAndRotate(Transform target)
        {
            toPos = target.position;
            toRot = target.rotation;
            isStart = true;
        }
    }
}
