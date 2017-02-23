using Core;
using Define;
using Model;
using System;

namespace Controller
{
    public class KitchenCtrl : Singleton<KitchenCtrl>
    {
        public KitchenModel Model = new KitchenModel();

        public void MovePos(int index, Action callback)
        {
            Model.Refresh(EventType.MoveCameraPos, index, callback);
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