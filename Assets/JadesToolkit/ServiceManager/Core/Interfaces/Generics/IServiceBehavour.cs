using UnityEngine;
namespace JadesToolkit.Services.Interfaces
{
    public interface IServiceBehavour<T> : IServiceBehaviour where T : MonoBehaviour
    {
        bool TryGetService(out T service);
    }
}