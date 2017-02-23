﻿using Core.Manager;
using System.Collections;
using UnityEngine;
using Controller;

namespace View.Kitchen
{
    public class FridgeDoorView : Core.MVC.EntityView
    {
        [SerializeField]
        int index;

        [SerializeField]
        Transform hand;

        Vector3 lastPos;

        Vector3 currPos;

        [SerializeField]
        Transform fridgePos;

        [SerializeField]
        Transform fridgePosY;

        Vector3 nVec;

        [SerializeField]
        float maxAngle = -75f;

        float startDelay = 0.15f;

        float endDelay = 0.5f;

        float timer = 0;

        IEnumerator coroutine;

        Vector3 lastEuler;

        bool[] isDoorOpened = new bool[] { false, false };

        bool[] isFingerExtended = new bool[] { true, true };

        int handType = -1;

        bool isWaitForClear = false;

        public void ExtendFingers(params object[] arg1)
        {
            int direction = (int)arg1[0];
            isFingerExtended[direction] = true;
        }

        public void UnextendFingers(params object[] arg1)
        {
            int direction = (int)arg1[0];
            isFingerExtended[direction] = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Contains("Finger"))
            {
                if(other.tag.Contains("Left"))
                {
                    if (isFingerExtended[0] == true)
                        return;
                }
                else if(other.tag.Contains("Right"))
                {
                    if (isFingerExtended[1] == true)
                        return;
                }

                if (hand == null)
                {
                    hand = other.transform;
                    lastPos = currPos = hand.position;
                    lastEuler = ChangeEulerAngle(gameObject.transform.localRotation.eulerAngles);
                    timer = 0;

                    if (other.tag.Contains("Left"))
                    {
                        handType = 0;
                    }
                    else if (other.tag.Contains("Right"))
                    {
                        handType = 1;
                    }
                }
            }
        }

        IEnumerator ReleaseHand()
        {
            while (true)
            {
                while (!isWaitForClear)
                    yield return new WaitForSeconds(0.05f);

                float time = 0;
                while (time < endDelay && isWaitForClear)
                {
                    time += 0.05f;
                    yield return new WaitForSeconds(0.05f);
                }
                hand = null;
                handType = -1;
                isWaitForClear = false;
            }
        }

        void Start()
        {
            coroutine = ReleaseHand();
            CoroutineManager.Instance.StartCoroutine(coroutine);

            nVec = (fridgePosY.position - fridgePos.position).normalized;
            Init(KitchenCtrl.Instance.Model);
            Bind(Define.EventType.ExtendFingers, ExtendFingers);
            Bind(Define.EventType.UnextendFingers, UnextendFingers);
        }

        void Update()
        {
            if (hand != null)
            {
                if(hand.gameObject.activeInHierarchy == false || isFingerExtended[handType] == true)
                {
                    hand = null;
                    handType = -1;
                    isWaitForClear = false;
                    return;
                }

                if (timer < startDelay)
                {
                    timer += Time.deltaTime;
                    return;
                }

                lastPos = currPos;
                currPos = hand.position;

                Vector3 lastVec = lastPos - fridgePos.position;
                float lastHeight = Vector3.Dot(lastVec, nVec);
                Vector3 lastProjVec = new Vector3(lastPos.x, lastPos.y - lastHeight, lastPos.z);
                Vector3 currVec = currPos - fridgePos.position;
                float currHeight = Vector3.Dot(currVec, nVec);
                Vector3 currProjVec = new Vector3(currPos.x, currPos.y - currHeight, currPos.z);

                Vector2 from = new Vector2((fridgePos.position - lastProjVec).x, (fridgePos.position - lastProjVec).z);
                Vector2 to = new Vector2((fridgePos.position - currProjVec).x, (fridgePos.position - currProjVec).z);
                float rotateAngle = VectorAngle(from, to);
             
                Vector3 euler = ChangeEulerAngle(gameObject.transform.localRotation.eulerAngles);
                lastEuler = euler;
                if (euler.y - rotateAngle < maxAngle || euler.y - rotateAngle > 0)
                    return;

                Quaternion q = Quaternion.Euler(euler.x, euler.y - rotateAngle, euler.z);
                gameObject.transform.localRotation = q;
                CheckOpenState();
            }
        }

        private float VectorAngle(Vector2 from, Vector2 to)
        {
            float angle;
            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            return cross.z < 0 ? -angle : angle;
        }

        private void CheckOpenState()
        {
            float angle = ChangeEulerAngle(gameObject.transform.localRotation.eulerAngles).y;
            if ((angle < -85f && !isDoorOpened[index]) || (angle > -85f && lastEuler.y < -85f && isDoorOpened[index]))
            {
                KitchenCtrl.Instance.ChangeFridgeDoor(index);
                isDoorOpened[index] = !isDoorOpened[index];
            }
        }

        private Vector3 ChangeEulerAngle(Vector3 euler)
        {
            float x = euler.x < 180 ? euler.x : euler.x - 360;
            float y = euler.y < 180 ? euler.y : euler.y - 360;
            float z = euler.z < 180 ? euler.z : euler.z - 360;
            return new Vector3(x, y, z);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            CoroutineManager.Instance.StartCoroutine(coroutine);
        }
    }
}
