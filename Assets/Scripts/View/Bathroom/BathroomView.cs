using Controller;
using Core.MVC;

namespace View.Bathroom
{
    public class BathroomView : EntityView
    {
        void Start()
        {
            Init(BathroomCtrl.Instance.Model);
        }
    }
}
