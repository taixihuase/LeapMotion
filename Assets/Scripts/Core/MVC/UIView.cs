using Core.Manager;
using Define;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.MVC
{
    public class UIView : View
    {
        [SerializeField]
        protected GameObject[] pos;

        [SerializeField]
        protected Button[] btn;

        [SerializeField]
        protected Image[] btnImage;

        [SerializeField]
        protected Button[] testBtn;

        protected Text[] btnText;

        protected Color normalUIColor = new Color(1f, 200f / 255, 100f / 255, 225f / 255);

        protected Color greenUIColor = new Color(125f / 255, 1f, 100f / 255, 225f / 255);

        protected Color normalTextColor = Color.black;

        protected Color redTextColor = new Color(150f / 255, 0, 0, 1f);

        protected override void Awake()
        {
            base.Awake();
            EventManager.Instance.AddEventHandler(Define.EventType.OpModeChanged, OnConnectedStateChanged);

            for (int i = 0; i < btn.Length; i++)
            {
                BindSound(btn[i], EventTriggerType.PointerDown, SoundType.Effect, "ButtonDown", false, 0.5f);
                BindSound(btn[i], EventTriggerType.PointerUp, SoundType.Effect, "ButtonUp", false, 0.5f);
            }
            btnText = new Text[btnImage.Length];
            for (int i = 0; i < btnImage.Length; i++)
            {
                btnText[i] = btnImage[i].transform.GetChild(0).GetComponent<Text>();
            }
            for (int i = 0; i < testBtn.Length; i++)
            {
                testBtn[i].image.color = Vector4.zero;
            }

            if (GlobalManager.Instance.IsConnected)
            {
                for (int i = 0; i < testBtn.Length; i++)
                {
                    testBtn[i].gameObject.SetActive(false);
                }
            }

            if (pos.Length > 0)
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    Canvas canvas = pos[i].GetComponent<Canvas>();
                    canvas.worldCamera = CameraManager.Instance.Camera;
                }
            }

            pos[0].SetActive(true);
            for (int i = 1; i < pos.Length; i++)
            {
                pos[i].SetActive(false);
            }

            ChangeNormalUIColor();
        }

        private void OnConnectedStateChanged(params object[] arg1)
        {
            for (int i = 0; i < testBtn.Length; i++)
            {
                testBtn[i].gameObject.SetActive(!(bool)arg1[0]);
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
            if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.ThrillingMode)
            {
                for (int i = 0; i < btnImage.Length; i++)
                {
                    btnImage[i].color = greenUIColor;
                    btnText[i].color = redTextColor;
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventManager.Instance.RemoveEventHandler(Define.EventType.OpModeChanged, OnConnectedStateChanged);
        }

        protected void BindSound(Button btn, EventTriggerType triggerType, SoundType soundType, string soundName, bool isLoop = false, float volumn = 1f)
        {
            EventTrigger trigger = btn.GetComponent<EventTrigger>();
            if (trigger == null)
                return;

            EventTrigger.Entry entry = trigger.triggers.Find(t => t.eventID == triggerType);
            if (entry == null)
            {
                entry = new EventTrigger.Entry();
                entry.eventID = triggerType;
                trigger.triggers.Add(entry);
            }
            entry.callback.AddListener((data) =>
            {
                OnPointerDelegate((PointerEventData)data, soundType, soundName, isLoop, volumn);
            });
        }

        private void OnPointerDelegate(PointerEventData data, SoundType soundType, string soundName, bool isLoop, float volumn)
        {
            if(soundType == SoundType.Effect)
                SoundManager.Instance.PlayEffectSound(soundName, isLoop, volumn);
            else
                SoundManager.Instance.PlayEnvironmentSound(soundName, isLoop, volumn);
        }
    }
}
