using UnityEngine;
namespace JadesToolkit.Services
{
    public abstract class ServiceBase : MonoBehaviour
    {
        /// <summary>
        /// Abstract method used for initialization of service.
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Attempts to fetch an initialized instance of the specified service.
        /// </summary>
        /// <param name="obj">Object reference of service.</param>
        /// <returns name="bool">Returns true if an object instance exists, false otherwise.</returns>
        public abstract bool TryGetService(out object obj);

    }
}