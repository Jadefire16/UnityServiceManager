using UnityEngine;

namespace JadesToolkit
{
    public abstract class SingletonBase<T> : MonoSingleton where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                else
                {
                    Debug.LogError($"Created an emergency copy of {typeof(T)} Type was no initialized!");
                    instance = new GameObject($"Instance of {typeof(T).Name}").AddComponent<T>();
                }
                return instance;
            }
            protected set
            {
                instance = value;
            }
        }
        public override void Initialize()
        {
            if (instance != null)
                return;
            instance = GetComponent<T>();
            if (instance == null)
            {
                instance = gameObject.AddComponent<T>();
                throw new InvalidStateException($"Specified Instance was null! This likely means the {nameof(SingletonManager)} failed to register this component!");
            }
        }

    }
}