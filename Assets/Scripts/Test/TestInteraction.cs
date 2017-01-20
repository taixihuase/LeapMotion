using UnityEngine;
using Core.Manager;

namespace Test
{
    public class TestInteraction : MonoBehaviour
    {
        private void Awake()
        {
            var interaction = InteractionManager.Instance;
        }
    }
}
