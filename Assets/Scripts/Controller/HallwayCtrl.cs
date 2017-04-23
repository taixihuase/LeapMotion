using Model;

namespace Controller
{
    public class HallwayCtrl : Core.MVC.Controller<HallwayCtrl>
    {
        protected override void Init()
        {
            base.Init();
            model = new HallwayModel();
        }
    }
}
