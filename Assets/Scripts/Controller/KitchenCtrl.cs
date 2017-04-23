using Define;
using Model;

namespace Controller
{
    public class KitchenCtrl : Core.MVC.Controller<KitchenCtrl>
    {
        protected override void Init()
        {
            base.Init();
            model = new KitchenModel();
        }

        public void ChangeFire(int index)
        {
            Model.Refresh(EventType.FireSwitchChanged, index);
        }

        public void ChangeFridgeDoor(int index)
        {
            Model.Refresh(EventType.FridgeDoorChanged, index);
        }

        public void ChangeLight()
        {
            Model.Refresh(EventType.KitchenLightChanged);
        }

        public void UnextendFingers(int direction)
        {
            Model.Refresh(EventType.UnextendFingers, direction);
        }

        public void ExtendFingers(int direction)
        {
            Model.Refresh(EventType.ExtendFingers, direction);
        }
    }
}