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

        public void OpenFridgeDoor(int index)
        {
            Model.Refresh(EventType.FridgeDoorOpened, index);
        }
    }
}
