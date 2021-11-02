using JadesToolkit.Services.Exceptions;
using JadesToolkit.Services.Interfaces;
using UnityEngine;

namespace JadesToolkit.Services
{
    public abstract class ServiceBase<T> : ServiceBase, IServiceBehavour<T> where T : MonoBehaviour
    {
        private static T instance;
        public static bool Initialized { get; protected set; }
        public static ServiceState State { get; protected set; } = ServiceState.Inactive;

        /// <summary>
        /// Initializes the service of type <typeparamref name="T"/>
        /// </summary>
        public override void Initialize()
        {
            if (instance != null)
                return;
            instance = GetComponent<T>();
            if (instance == null)
            {
                instance = gameObject.AddComponent<T>();
                throw new InvalidStateException($"This service is currently not running! You need to start the service using ServiceManager.{nameof(ServiceManager.TryAddExternalService)} before using this service.");
            }
            Initialized = true;
            State = ServiceState.Idle;
        }

        /// <summary>
        /// Attempts to fetch the specified service.
        /// </summary>
        /// <param name="service"></param>
        /// <returns><see cref="bool"/> signifying whether the call was successful.</returns>
        public bool TryGetService(out T service)
        {
            service = instance;
            return service == null;
        }

        /// <summary>
        /// <inheritdoc cref="TryGetService(out T)"/>
        /// </summary>
        /// <param name="obj">An <see cref="object"/> representing the service.</param>
        /// <returns><inheritdoc cref="TryGetService(out T)"/></returns>
        public override bool TryGetService(out object obj)
        {
            obj = instance;
            return obj == null;
        }

        protected virtual void OnDestroy()
        {
            ServiceManager.TryRemoveService(this.GetType());
            instance = null;
        }
    }
}