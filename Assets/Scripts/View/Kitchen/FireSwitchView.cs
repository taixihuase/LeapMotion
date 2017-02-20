using Controller;
using UnityEngine;

namespace View.Kitchen
{
    public class FireSwitchView : Core.MVC.View
    {
        [SerializeField]
        int index;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Hand")
            {
                KitchenCtrl.Instance.ChangeFire(index);
            }
        }
    }
}
