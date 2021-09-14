using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace JadesToolkit
{
    public static class ServiceManager
    {
        private static readonly Dictionary<Type, ServiceBase> services = new Dictionary<Type, ServiceBase>();

        private static GameObject coreObject;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            services.Clear();
            coreObject = new GameObject("Service Manager");
            Assembly assembly = Assembly.GetAssembly(typeof(ServiceBase)); 

            var allServiceTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ServiceBase)) && t.IsAbstract == false).ToArray(); // fetch all MonoSingleton types and store in an array

            foreach (var service in allServiceTypes)
            {
                if (service.IsDefined(typeof(DoNotInitializOnLoadAttribute), false))
                    continue;
                var component = coreObject.AddComponent(service) as ServiceBase;
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
        public static bool TryGetService<T>(out T value) where T : ServiceBase
        {
            value = null;
            bool success = services.TryGetValue(typeof(T), out ServiceBase val);
            if (!success)
                return false;
            value = val as T;
            return true;
        }

        /// <summary>
        /// This will attempt to immediately fetch a loaded singleton of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T where T is a MonoSingleton.</returns>
        public static T GetService<T>() where T : ServiceBase => services[typeof(T)] as T;

        public static T GetOrStartService<T>() where T : ServiceBase
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
        public static bool TryEndService<T>() where T : ServiceBase
        {
            if (!TryGetService<T>(out T value))
                return false;
            services.Remove(typeof(T));
            GameObject.Destroy(value);
            return true;
        }

        /// <summary>
        /// Attempts to end the service of type T
        /// </summary>
        /// <typeparam name="T">Type of Service</typeparam>
        public static void EndService<T>() where T : ServiceBase
        {
            var service = services[typeof(T)];
            services.Remove(typeof(T));
            GameObject.Destroy(service);
        }

        /// <summary>
        /// Attempts to start a service specified by type T
        /// </summary>
        /// <typeparam name="T">The type of the service</typeparam>
        /// <returns>Returns true is service was successfully started, false otherwise.</returns>
        public static bool StartService<T>() where T : ServiceBase
        {
            if (services.ContainsKey(typeof(T)))
                return false;
            var component = coreObject.AddComponent(typeof(T)) as ServiceBase;
            component.Initialize();
            services.Add(typeof(T), component);
            return true;
        }

        /// <summary>
        /// Starts a new service of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T where T is ServiceBase</returns>
        /// <remarks>Note: This will forcibly terminate an active instance of this type and create a new one</remarks>
        public static T ForceStartService<T>() where T : ServiceBase
        {
            if(services.ContainsKey(typeof(T)))
                EndService<T>();              

            var component = coreObject.AddComponent(typeof(T)) as ServiceBase;
            component.Initialize();
            services.Add(typeof(T), component);
            return component as T;
        }
    }
}