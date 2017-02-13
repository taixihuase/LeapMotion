using Core;
using Model;

namespace Controller
{
    public class KitchenCtrl : Singleton<KitchenCtrl>
    {
        public KitchenModel Model = new KitchenModel();
    }
}
