using UnityEngine;

namespace Test
{
    public class TestTouch : MonoBehaviour
    {
        private void OnTriggerEnter(Collider obj)
        {
            Debug.Log(obj.transform.parent.name);
            transform.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
