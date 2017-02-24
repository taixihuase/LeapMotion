using Controller;
using UnityEngine;

namespace View.Bathroom
{
    public class WaterOutToggleView : Core.MVC.EntityView
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Hand" || other.tag.Contains("Finger"))
            {
                BathroomCtrl.Instance.ChangeWaterOutToggle();
            }
        }
    }
}
