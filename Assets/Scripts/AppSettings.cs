using UnityEngine;

public class AppSettings : MonoBehaviour
{
    private void Start()
    {
#if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}