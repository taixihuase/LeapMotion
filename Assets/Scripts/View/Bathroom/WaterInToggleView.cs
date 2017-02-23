using Controller;
using UnityEngine;

namespace View.Bathroom
{
    public class WaterInToggleView : Core.MVC.EntityView
    {
        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Hand")
            {
                BathroomCtrl.Instance.ChangeWaterInToggle();
            }
        }
    }
}
