using Controller;
using UnityEngine;

namespace View.Kitchen
{
    public class FireSwitchView : Core.MVC.EntityView
    {
        [SerializeField]
        int index;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Hand" || other.tag.Contains("Finger"))
            {
                KitchenCtrl.Instance.ChangeFire(index);
            }
        }
    }
}
