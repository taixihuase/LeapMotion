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

        protected Text[] btnText;

        protected Color normalUIColor = new Color(1f, 200f / 255, 100f / 255, 225f / 255);

        protected Color greenUIColor = new Color(125f / 255, 1f, 100f / 255, 225f / 255);

        protected Color normalTextColor = Color.black;

        protected Color redTextColor = new Color(150f / 255, 0, 0, 1f);

        protected override void Awake()
        {
            base.Awake();
            if (btnImage.Length > 0)
            {
                btnText = new Text[btnImage.Length];
                for (int i = 0; i < btnImage.Length; i++)
                {
                    btnText[i] = btnImage[i].transform.GetChild(0).GetComponent<Text>();
                }
            }
        }

        protected void ChangeNormalUIColor()
        {
            for (int i = 0; i < btnImage.Length; i++)
            {
                btnImage[i].color = normalUIColor;
                btnText[i].color = normalTextColor;
            }
        }

        protected void ChangeGreenUIColor()
        {
            for (int i = 0; i < btnImage.Length; i++)
            {
                btnImage[i].color = greenUIColor;
                btnText[i].color = redTextColor;
            }
        }
    }
}
