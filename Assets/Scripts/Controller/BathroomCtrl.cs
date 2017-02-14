﻿using Core;
using Define;
using Model;

namespace Controller
{
    public class BathroomCtrl : Singleton<BathroomCtrl>
    {
        public BathroomModel Model = new BathroomModel();

        public void MovePos(int index)
        {
            Model.Refresh(EventType.MoveCameraPos, index);
        }
    }
}
