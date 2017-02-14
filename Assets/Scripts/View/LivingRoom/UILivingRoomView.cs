﻿using Controller;
using UnityEngine;

namespace View.Living
{
    public class UILivingRoomView : Core.MVC.View
    {
        [SerializeField]
        GameObject[] pos;

        void Start()
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (i != 0)
                    pos[i].SetActive(false);
            }
        }

        public void OnClickToPos1()
        {
            LivingRoomCtrl.Instance.MovePos(0);
            pos[1].SetActive(false);
            pos[0].SetActive(true);
        }

        public void OnClickToPos2()
        {
            LivingRoomCtrl.Instance.MovePos(1);
            pos[0].SetActive(false);
            pos[2].SetActive(false);
            pos[1].SetActive(true);
        }

        public void OnClickToPos3()
        {
            LivingRoomCtrl.Instance.MovePos(2);
            pos[1].SetActive(false);
            pos[2].SetActive(true);
        }

        public void OnClickToHallway()
        {

        }
    }
}
