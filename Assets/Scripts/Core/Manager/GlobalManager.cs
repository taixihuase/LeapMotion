using UnityEngine;

namespace Core.Manager
{
    public sealed class GlobalManager : Singleton<GlobalManager>
    {
        public void EnableSettings()
        {
#if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
            Object.DontDestroyOnLoad(CameraManager.Instance.Camera.gameObject);
        }

        public enum Mode
        {
            PracticeMode = 1,
            ThrillingMode = 2
        }

        public Mode SceneMode { get; set; }
    }
}
