﻿using Core;
using Define;
using Model;
using System;

namespace Controller
{
    public class BathroomCtrl : Singleton<BathroomCtrl>
    {
        public BathroomModel Model = new BathroomModel();

        public void MovePos(int index, Action callback)
        {
            Model.Refresh(EventType.MoveCameraPos, index, callback);
        }

        public void ChangeWaterInToggle()
        {
            Model.Refresh(EventType.WaterInToggleChanged);
        }

        public void ChangeWaterOutToggle()
        {
            Model.Refresh(EventType.WaterOutToggleChanged);
        }
    }
}
