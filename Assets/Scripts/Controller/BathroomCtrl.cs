using Core;
using Model;

namespace Controller
{
    public class BathroomCtrl : Singleton<BathroomCtrl>
    {
        public BathroomModel Model = new BathroomModel();
    }
}
