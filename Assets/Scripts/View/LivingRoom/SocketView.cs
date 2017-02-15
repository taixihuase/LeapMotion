﻿using Controller;
using UnityEngine;

namespace View.LivingRoom
{
    public class SocketView : Core.MVC.View
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Plug")
            {
                LivingRoomCtrl.Instance.InsertPlug();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.tag == "Plug")
            {
                LivingRoomCtrl.Instance.PutPlugOut();
            }
        }
    }
}
