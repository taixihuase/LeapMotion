using Core;
using Define;
using Model;

namespace Controller
{
    public class LivingRoomCtrl : Singleton<LivingRoomCtrl>
    {
        public LivingRoomModel Model = new LivingRoomModel();

        public void MovePos(int index)
        {
            Model.Refresh(EventType.MoveCameraPos, index);
        }
    }
}
