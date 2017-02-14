using Core.Manager;
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
            CameraManager.Instance.MoveAndRotate(pos[index]);
        }

        public Transform GetStartPos()
        {
            return pos[0];
        }
    }
}
