using Controller;
using Core.MVC;

namespace View.Kitchen
{
    public class KitchenView : EntityView
    {
        void Start()
        {
            Init(KitchenCtrl.Instance.Model);
        }
    }
}
