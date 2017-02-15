using Controller;
using UnityEngine;

namespace View.Bathroom
{
    public class WaterOutToggleView : Core.MVC.View
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Hand")
            {
                BathroomCtrl.Instance.ChangeWaterOutToggle();
            }
        }
    }
}
