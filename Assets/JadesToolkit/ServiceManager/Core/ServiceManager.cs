using JadesToolkit.Services.Attributes;
using JadesToolkit.Services.Interfaces;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace JadesToolkit.Services
{
    public static class ServiceManager
    {
        private static readonly Dictionary<Type, IServiceBehaviour> services = new Dictionary<Type, IServiceBehaviour>();

        private static GameObject coreObject;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            services.Clear();
            coreObject = new GameObject("Service Manager");
            Assembly assembly = Assembly.GetAssembly(typeof(IServiceBehaviour));

            var allServiceTypes = assembly.GetTypes().Where(t => typeof(IServiceBehaviour).IsAssignableFrom(t) && t.IsClass && t.IsSubclassOf(typeof(MonoBehaviour)) && !t.IsAbstract).ToArray(); // fetch all MonoSingleton types and store in an array

            foreach (var service in allServiceTypes)
            {
                if (service.IsDefined(typeof(DoNotInitializeOnLoadAttribute), false))
                    continue;
                var component = coreObject.AddComponent(service) as IServiceBehaviour;
                component.Initialize();
                services.Add(service, component);
            }
            UnityEngine.Object.DontDestroyOnLoad(coreObject);
        }

        /// <summary>
        /// Attempts to fetch a service of type T.
        /// </summary>
        /// <typeparam name="T">The type of service</typeparam>
        /// <param name="value">The service</param>
        /// <returns>True if a service was found, otherwise false.</returns>
        public static bool TryGetService<T>(out T value) where T : MonoBehaviour, IServiceBehaviour
        {
            value = default;
            bool success = services.TryGetValue(typeof(T), out IServiceBehaviour val);
            if (!success)
                return false;
            value = val as T;
            return true;
        }

        /// <summary>
        /// This will attempt to immediately fetch a loaded singleton of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns name="T"><typeparamref name="T"/> where <typeparamref name="T"/> is type of <typeparamref name="IServiceBehaviour"/>.</returns>
        public static T GetService<T>() where T : MonoBehaviour, IServiceBehaviour => services[typeof(T)] as T;

        /// <summary>
        /// This will immediately fetch a loaded singleton of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <remarks>If a service is not already active of type T one will be started</remarks>
        /// <returns name="T"><typeparamref name="T"/> where <typeparamref name="T"/> is type of <typeparamref name="IServiceBehaviour"/>.</returns>
        public static T GetRequiredService<T>() where T : MonoBehaviour, IServiceBehaviour
        {
            if (TryGetService<T>(out T value))
                return value;
            return ForceStartService<T>();
        }


        /// <summary>
        /// Attempts to end the service of type T
        /// </summary>
        /// <typeparam name="T">Type of Service</typeparam>
        /// <returns>True if service was ended, otherwise false.</returns>
        public static bool TryEndService<T>() where T : MonoBehaviour, IServiceBehaviour
        {
            if (!TryGetService<T>(out T value))
                return false;
            services.Remove(typeof(T));
            UnityEngine.Object.Destroy(value);
            return true;
        }

        /// <summary>
        /// Attempts to end the service of type T and destroys the object
        /// </summary>
        /// <typeparam name="T">Type of Service</typeparam>
        public static void EndService<T>() where T : MonoBehaviour, IServiceBehaviour
        {
            if (!services.ContainsKey(typeof(T)))
                return;
            var service = services[typeof(T)] as T;
            services.Remove(typeof(T));
            UnityEngine.Object.Destroy(service);
        }

        /// <summary>
        /// Removes the service from the service managers control.
        /// </summary>
        /// <typeparam name="T">Type of Service</typeparam>
        public static void TryRemoveService<T>() where T : MonoBehaviour, IServiceBehaviour
        {
            if (!services.ContainsKey(typeof(T)))
                return;
            services.Remove(typeof(T));
        }

        /// <summary>
        /// Removes the service from the service managers control.
        /// </summary>
        /// <typeparam name="T">Type of Service</typeparam>
        public static void TryRemoveService(Type t)
        {
            if (!services.ContainsKey(t))
                return;
            services.Remove(t);
        }

        /// <summary>
        /// Attempts to start a service specified by type T
        /// </summary>
        /// <typeparam name="T">The type of the service</typeparam>
        /// <returns>Returns true is service was successfully started, false otherwise.</returns>
        public static bool StartService<T>() where T : MonoBehaviour, IServiceBehaviour
        {
            if (services.ContainsKey(typeof(T)))
                return false;
            var component = coreObject.AddComponent(typeof(T)) as IServiceBehaviour;
            component.Initialize();
            services.Add(typeof(T), component);
            return true;
        }

        /// <summary>
        /// Starts a new service of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T where T is IServiceBehaviour</returns>
        /// <remarks>Note: This will forcibly terminate an active instance of this type and create a new one</remarks>
        public static T ForceStartService<T>() where T : MonoBehaviour, IServiceBehaviour
        {
            if(services.ContainsKey(typeof(T)))
                EndService<T>();              

            var component = coreObject.AddComponent(typeof(T)) as IServiceBehaviour;
            component.Initialize();
            services.Add(typeof(T), component);
            return component as T;
        }

        /// <summary>
        /// Integrates an external service into the service manager.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T where T is IServiceBehaviour</returns>
        /// <remarks>This is useful for services marked with <typeparamref name="DoNotInitializeOnLoad"/></remarks>
        public static bool TryAddExternalService<T>(IServiceBehaviour service, bool initialize = false) where T : MonoBehaviour, IServiceBehaviour
        {
            if (services.ContainsKey(typeof(T)))
                return false;
            services.Add(typeof(T), service);
            if (initialize)
                service.Initialize();
            return true;
        }
    }
}