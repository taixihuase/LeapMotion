using System.Collections;
using UnityEngine;

namespace Core.Manager
{
    public sealed class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        public new Coroutine StartCoroutine(IEnumerator func)
        {
            return base.StartCoroutine(func);
        }

        public new void StopCoroutine(IEnumerator func)
        {
            base.StopCoroutine(func);
        }

        public new void StopAllCoroutines()
        {
            base.StopAllCoroutines();
        }
    }
}
