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
                throw new InvalidStateException($"This service is currently not running! You need to start the service using ServiceManager. StartService<{typeof(T).Name}>() before using this service.");
            }
            Initialized = true;
        }
    }
}