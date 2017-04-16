using Leap.Unity;
using UnityEngine;

namespace Core.Manager
{
    public sealed class GlobalManager : MonoSingleton<GlobalManager>
    {
        public void EnableSettings()
        {
#if UNITY_EDITOR
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
#else
            if (isConnected)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
#endif
            if (CameraManager.Instance.Camera != null)
            {
                DontDestroyOnLoad(CameraManager.Instance.Camera.gameObject);
            }
        }

        public enum Mode
        {
            PracticeMode = 0,
            ThrillingMode = 1,
        }

        public Mode SceneMode { get; set; }

        private bool initState = false;

        public bool InitState
        {
            get { return initState; }
        }

        public void Init()
        {
            provider = LeapMotionManager.Instance.Provider as LeapServiceProvider;
            isConnected = provider.IsConnected();
            EnableSettings();
            initState = true;
        }

        private LeapServiceProvider provider;

        private bool isConnected = false;

        public bool IsConnected { get { return isConnected; } }

        private void Update()
        {
            if (isConnected != provider.IsConnected())
            {
                isConnected = !isConnected;
                EventManager.Instance.RaiseEvent(Define.EventType.OpModeChanged, isConnected);
                if (isConnected)
                {
#if !UNITY_EDITOR
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
#endif
                }
                else
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }

        private void OnApplicationQuit()
        {
            SoundManager.Instance.StopEnvironmentSound(true);
            SoundManager.Instance.StopEffectSound(true);
        }
    }
}
