using Controller;
using Core.MVC;
using UnityEngine;

namespace View.LivingRoom
{
    public class SocketView : EntityView
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Plug")
            {
                LivingRoomCtrl.Instance.InsertPlug();
            }
            else if (other.tag == "Hand" || other.tag.Contains("Finger"))
            {
                LivingRoomCtrl.Instance.ElectricWarning();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Plug")
            {
                LivingRoomCtrl.Instance.PutPlugOut();
            }
            else if (other.tag == "Hand" || other.tag.Contains("Finger"))
            {
                LivingRoomCtrl.Instance.CancelWarning();
            }
        }
    }
}
