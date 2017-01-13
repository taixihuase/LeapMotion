using System;
using System.Collections;
using UnityEngine;

namespace Core.Manager
{
    public class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        public Coroutine StartCoroutine(IEnumerator func)
        {
            return base.StartCoroutine(func);
        }

        public void StopCoroutine(IEnumerator func)
        {
            base.StopCoroutine(func);
        }

        public void StopAllCoroutines()
        {
            base.StopAllCoroutines();
        }
    }
}
