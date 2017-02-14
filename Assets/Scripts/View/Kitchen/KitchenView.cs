using Controller;
using Core.MVC;
using Define;

namespace View.Kitchen
{
    public class KitchenView : EntityView
    {
        void Start()
        {
            Init(KitchenCtrl.Instance.Model);
            Bind(EventType.MoveCameraPos, MoveCamera);
        }
    }
}
