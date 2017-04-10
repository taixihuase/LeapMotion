using Core.Manager;
using UnityEngine;

public class AppStart : MonoBehaviour
{
    private void Start()
    {
        if (GlobalManager.Instance.InitState == false)
        {
            GlobalManager.Instance.Init();
        }
    }
}