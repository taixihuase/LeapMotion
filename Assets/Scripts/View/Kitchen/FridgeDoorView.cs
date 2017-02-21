using Core.Manager;
using System.Collections;
using UnityEngine;
using Controller;
using DG.Tweening;

namespace View.Kitchen
{
    public class FridgeDoorView : Core.MVC.View
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

        float endDelay = 0.3f;

        float timer = 0;

        IEnumerator coroutine;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Finger")
            {
                if (hand == null)
                {
                    hand = other.transform;
                    lastPos = currPos = hand.position;
                    timer = 0;
                }
            }
        }


        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Finger" && other.transform == hand)
            {
                coroutine = ReleaseHand();
                CoroutineManager.Instance.StartCoroutine(coroutine);
            }
        }

        IEnumerator ReleaseHand()
        {
            float time = 0;
            while (time < endDelay)
            {
                time += 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
            hand = null;
        }

        void Start()
        {
            nVec = (fridgePosY.position - fridgePos.position).normalized;
        }


        void Update()
        {
            if (hand != null)
            {
                if(hand.gameObject.activeInHierarchy == false)
                {
                    hand = null;
                    CoroutineManager.Instance.StopCoroutine(coroutine);
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
            if (ChangeEulerAngle(gameObject.transform.localRotation.eulerAngles).y < -60f)
            {
                //KitchenCtrl.Instance.OpenFridgeDoor(index);
            }
        }

        private Vector3 ChangeEulerAngle(Vector3 euler)
        {
            float x = euler.x < 180 ? euler.x : euler.x - 360;
            float y = euler.y < 180 ? euler.y : euler.y - 360;
            float z = euler.z < 180 ? euler.z : euler.z - 360;
            return new Vector3(x, y, z);
        }
    }
}
