using Core.MVC;
using Core;

namespace Controller
{
    public class TestMonoCtrl : MonoController<TestMonoCtrl>
    {
        private int test;

        public int Test
        {
            get { return test; }
            set { test = value; }
        }
    }

    public class TestCtrl : Controller<TestCtrl>
    {
        private int test;

        public int Test
        {
            get { return test; }
            set { test = value; }
        }
    }

    public class TestMonoSingleton : MonoSingleton<TestMonoSingleton>
    {
        private int test;

        public int Test
        {
            get { return test; }
            set { test = value; }
        }
    }

    public class TestSingleton : Singleton<TestSingleton>
    {
        private int test;

        public int Test
        {
            get { return test; }
            set { test = value; }
        }
    }
}