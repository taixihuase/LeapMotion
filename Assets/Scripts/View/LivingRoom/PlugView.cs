using System.Collections;
using UnityEngine;

namespace View.LivingRoom
{
    public class PlugView : Core.MVC.EntityView
    {
        private bool isDrag = false;

        public bool IsDrag
        {
            get
            {
                return isDrag;
            }
            set
            {
                isDrag = value;
            }
        }

        IEnumerator OnMouseDown()
        {
            Vector3 ScreenSpace = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenSpace.z));
            isDrag = true;
            while (Input.GetMouseButton(0) && isDrag)
            {
                Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenSpace.z);
                Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;      
                transform.position = CurPosition;
                yield return null;
            }
        }
    }
}