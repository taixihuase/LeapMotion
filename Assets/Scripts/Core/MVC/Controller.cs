using System;
using UnityEngine;
using EventType = Define.EventType;

namespace Core.MVC
{
    public class MonoController<T> : MonoSingleton<T> where T : MonoBehaviour
    {

    }

    public class Controller<T> : Singleton<T> where T : class, new()
    {
        protected Model model;

        public Model Model { get { return model; } }

        public void MovePos(int index, Action callback)
        {
            Model.Refresh(EventType.MoveCameraPos, index, callback);
        }
    }
}
