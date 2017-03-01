using Core;
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

        public void PourWater()
        {
            Model.Refresh(EventType.PourWater);
        }

        public void FillWater()
        {
            Model.Refresh(EventType.FillWater);
        }

        public void PourWaterBegin()
        {
            Model.Refresh(EventType.PourWaterBegin);
        }

        public void FillWaterBegin()
        {
            Model.Refresh(EventType.FillWaterBegin);
        }
    }
}
