using UnityEngine;

namespace JadesToolkit
{
    public abstract class ServiceBase<T> : ServiceBase where T : MonoBehaviour
    {
        private static T instance;
        public static bool Initialized { get; protected set; }
        public override void Initialize()
        {
            if (instance != null)
                return;
            instance = GetComponent<T>();
            if (instance == null)
            {
                instance = gameObject.AddComponent<T>();
                throw new InvalidStateException($"Specified Instance was null! This likely means the {nameof(ServiceManager)} failed to register this component!");
            }
            Initialized = true;
        }

    }
}