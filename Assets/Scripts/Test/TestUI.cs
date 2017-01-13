using UnityEngine;
using Core.Manager;

namespace Test
{
    public class TestUI : MonoBehaviour
    {
        private void Awake()
        {
            UIManager.Instance.Init();
        }
    }
}
