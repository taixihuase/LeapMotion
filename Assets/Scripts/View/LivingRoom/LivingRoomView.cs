using Controller;
using Core.MVC;
using Define;

namespace View.LivingRoom
{
    public class LivingRoomView : EntityView
    {
        void Start()
        {
            Init(LivingRoomCtrl.Instance.Model);
            Bind(EventType.MoveCameraPos, MoveCamera);
        }
    }
}
