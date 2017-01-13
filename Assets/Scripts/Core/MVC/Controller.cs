using UnityEngine;

namespace Core.MVC
{
    public class MonoController<T> : MonoSingleton<T> where T : MonoBehaviour
    {

    }

    public class Controller<T> : Singleton<T> where T : class, new()
    {

    }
}
