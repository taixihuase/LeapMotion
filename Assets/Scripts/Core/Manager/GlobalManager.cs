using Define;
using Leap.Unity;
using Tool;
using UnityEngine;

namespace Core.Manager
{
    public sealed class GlobalManager : Singleton<GlobalManager>
    {
        public void EnableSettings()
        {
#if UNITY_EDITOR
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
#else
            if ((LeapMotionManager.Instance.Provider as LeapServiceProvider).IsConnected())
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
#endif
            if (CameraManager.Instance.Camera != null)
            {
                Object.DontDestroyOnLoad(CameraManager.Instance.Camera.gameObject);
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
            EnableSettings();
            initState = true;
        }
    }
}
