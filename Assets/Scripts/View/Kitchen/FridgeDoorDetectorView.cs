using Controller;
using Core.Manager;
using Core.MVC;
using Leap.Unity;

namespace View.Kitchen
{
    public class FridgeDoorDetectorView : EntityView
    {
        public void OnActive(int direction)
        {
            KitchenCtrl.Instance.UnextendFingers(direction);
        }

        public void OnDisactive(int direction)
        {
            KitchenCtrl.Instance.ExtendFingers(direction);
        }

        ExtendedFingerDetector[] detectors;

        protected override void Awake()
        {
            detectors = GetComponents<ExtendedFingerDetector>();
            detectors[0].HandModel = HandManager.Instance.LeftHand;
            detectors[1].HandModel = HandManager.Instance.RightHand;

            HandManager.Instance.OnHandChanged += OnHandChanged;
        }

        private void OnHandChanged(IHandModel hand, int direction)
        {
            detectors[direction].HandModel = hand;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            HandManager.Instance.OnHandChanged -= OnHandChanged;
        }
    }
}
