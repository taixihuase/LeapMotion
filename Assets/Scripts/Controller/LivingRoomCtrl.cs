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

        public void PutPlugOut()
        {
            Model.Refresh(EventType.PutPlugOut);
        }
    }
}
