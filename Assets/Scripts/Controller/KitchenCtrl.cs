using Core;
using Define;
using Model;

namespace Controller
{
    public class KitchenCtrl : Singleton<KitchenCtrl>
    {
        public KitchenModel Model = new KitchenModel();

        public void MovePos(int index)
        {
            Model.Refresh(EventType.MoveCameraPos, index);
        }
    }
}
