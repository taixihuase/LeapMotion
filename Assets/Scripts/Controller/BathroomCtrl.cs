using Define;
using Model;

namespace Controller
{
    public class BathroomCtrl : Core.MVC.Controller<BathroomCtrl>
    {
        protected override void Init()
        {
            base.Init();
            model = new BathroomModel();
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
