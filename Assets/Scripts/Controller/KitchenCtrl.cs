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
    }
}
