using Core.Manager;
using UnityEngine;

public class AppStart : MonoBehaviour
{
    private void Start()
    {
        GlobalManager.Instance.EnableSettings();
    }
}