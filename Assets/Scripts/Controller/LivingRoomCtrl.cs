using Define;
using Model;

namespace Controller
{
    public class LivingRoomCtrl : Core.MVC.Controller<LivingRoomCtrl>
    {
        protected override void Init()
        {
            base.Init();
            model = new LivingRoomModel();
        }

        public void InsertPlug()
        {
            Model.Refresh(EventType.InsertPlug);
        }

        public void OnInsertPlugComplete()
        {
            Model.Refresh(EventType.InsertPlugComplete);
        }

        public void PutPlugOut()
        {
            Model.Refresh(EventType.PutPlugOut);
        }

        public void FixPlugPosition()
        {
            Model.Refresh(EventType.FixPlugPos);
        }

        public void ElectricWarning()
        {
            Model.Refresh(EventType.ElectricWarning);
        }

        public void CancelWarning()
        {
            Model.Refresh(EventType.CancelElectricWarning);
        }
    }
}
