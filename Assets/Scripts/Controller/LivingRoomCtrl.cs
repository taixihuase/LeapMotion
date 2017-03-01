using Core;
using Define;
using Model;
using System;

namespace Controller
{
    public class LivingRoomCtrl : Singleton<LivingRoomCtrl>
    {
        public LivingRoomModel Model = new LivingRoomModel();

        public void MovePos(int index, Action callback)
        {
            Model.Refresh(EventType.MoveCameraPos, index, callback);
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
