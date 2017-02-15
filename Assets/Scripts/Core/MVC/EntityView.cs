using Core.Manager;
using System;
using UnityEngine;

namespace Core.MVC
{
    public class EntityView : View
    {
        [SerializeField]
        protected Transform[] pos;

        protected void MoveCamera(params object[] arg1)
        {
            int index = (int)arg1[0];
            Action callback = null;
            if (arg1.Length >= 2)
            {
                callback = (Action)arg1[1];
            }
            CameraManager.Instance.MoveAndRotate(pos[index], callback);
        }

        public Transform GetStartPos()
        {
            return pos[0];
        }
    }
}
