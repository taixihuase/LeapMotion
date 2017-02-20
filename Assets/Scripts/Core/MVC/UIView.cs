using UnityEngine;
using UnityEngine.UI;

namespace Core.MVC
{
    public class UIView : View
    {
        [SerializeField]
        protected GameObject[] pos;

        [SerializeField]
        protected Image[] btnImage;

        protected Color normalUIColor = new Color(1f, 200f / 255, 100f / 255, 225f / 255);

        protected Color greenUIColor = new Color(125f / 255, 1f, 100f / 255, 225f / 255);

        protected void ChangeNormalUIColor()
        {
            for (int i = 0; i < btnImage.Length; i++)
            {
                btnImage[i].color = normalUIColor;
            }
        }

        protected void ChangeGreenUIColor()
        {
            for (int i = 0; i < btnImage.Length; i++)
            {
                btnImage[i].color = greenUIColor;
            }
        }
    }
}
