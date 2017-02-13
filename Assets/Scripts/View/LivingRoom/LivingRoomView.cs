using Controller;
using Core.MVC;

namespace View.LivingRoom
{
    public class LivingRoomView : EntityView
    {
        void Start()
        {
            Init(LivingRoomCtrl.Instance.Model);
        }
    }
}
