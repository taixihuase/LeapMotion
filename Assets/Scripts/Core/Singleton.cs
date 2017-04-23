using UnityEngine;

namespace Core
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name.ToString());
                    instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {    
        }
    }

    public abstract class Singleton<T>  where T : class, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null) { instance = new T(); }
                return instance;
            }
        }

        protected Singleton()
        {
            Init();
        }

        protected virtual void Init()
        {            
        }
    }
}