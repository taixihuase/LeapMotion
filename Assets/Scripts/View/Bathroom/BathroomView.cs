using Controller;
using Core.MVC;
using Define;

namespace View.Bathroom
{
    public class BathroomView : EntityView
    {
        void Start()
        {
            Init(BathroomCtrl.Instance.Model);
            Bind(EventType.MoveCameraPos, MoveCamera);
        }
    }
}
