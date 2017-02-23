using Controller;
using Core.Manager;
using Leap.Unity;
using UnityEngine;

namespace View.Kitchen
{
    public class LightDetectorView : Core.MVC.EntityView
    {
        float timer = 0;

        [SerializeField]
        float duration = 0.5f;

        int hit = 0;

        public void OnActive()
        {
            hit++;
        }

        private void ChangeLightState()
        {
            KitchenCtrl.Instance.ChangeLight();
        }

        ObjectProximityDetector proximityDetector;

        PalmDirectionDetector[] palmDetectors;

        protected override void Awake()
        {
            proximityDetector = GetComponent<ObjectProximityDetector>();
            FingerModel[] leftFingers = (HandManager.Instance.LeftHand as RiggedHand).fingers;
            FingerModel[] rightFingers = (HandManager.Instance.RightHand as RiggedHand).fingers;
            GameObject[] lf = new GameObject[leftFingers.Length];
            GameObject[] rf = new GameObject[rightFingers.Length];
            for(int i = 0; i < leftFingers.Length; i++)
            {
                lf[i] = leftFingers[i].gameObject;
            }
            for(int i = 0; i < rightFingers.Length; i++)
            {
                rf[i] = rightFingers[i].gameObject;
            }

            GameObjectArray[] fingers = new GameObjectArray[] { new GameObjectArray(lf), new GameObjectArray(rf) };
            proximityDetector.SetTargetObjects(fingers);

            palmDetectors = GetComponents<PalmDirectionDetector>();
            palmDetectors[0].HandModel = HandManager.Instance.LeftHand;
            palmDetectors[1].HandModel = HandManager.Instance.RightHand;

            HandManager.Instance.OnHandChanged += OnHandChanged;
        }

        private void OnHandChanged(IHandModel hand, int direction)
        {
            palmDetectors[direction].HandModel = hand;
            RiggedHand hm = hand as RiggedHand;
            if(direction == 0)
            {
                GameObject[] lf = new GameObject[hm.fingers.Length];
                for (int i = 0; i < hm.fingers.Length; i++)
                {
                    lf[i] = hm.fingers[i].gameObject;
                }
                GameObjectArray rf = proximityDetector.TargetObjects[1];
                GameObjectArray[] fingers = new GameObjectArray[] { new GameObjectArray(lf), rf };
                proximityDetector.SetTargetObjects(fingers);
            }
            else
            {
                GameObject[] rf = new GameObject[hm.fingers.Length];
                for (int i = 0; i < hm.fingers.Length; i++)
                {
                    rf[i] = hm.fingers[i].gameObject;
                }
                GameObjectArray lf = proximityDetector.TargetObjects[0];
                GameObjectArray[] fingers = new GameObjectArray[] { lf, new GameObjectArray(rf) };
                proximityDetector.SetTargetObjects(fingers);
            }
        }

        void Update()
        {
            if(hit > 0)
            {
                if(timer < duration)
                {
                    timer += Time.deltaTime;
                    if(hit == 2)
                    {
                        ChangeLightState();
                        timer = 0;
                        hit = 0;
                    }
                }
                else
                {
                    timer = 0;
                    hit = 0;
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            HandManager.Instance.OnHandChanged -= OnHandChanged;
        }
    }
}
