using Leap.Unity;
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

            Object.DontDestroyOnLoad(CameraManager.Instance.Camera.gameObject);
        }

        public enum Mode
        {
            PracticeMode = 0,
            ThrillingMode = 1,
        }

        public Mode SceneMode { get; set; }
    }
}
