
namespace JadesToolkit.Services.Interfaces
{
    public interface IServiceBehaviour
    {
        void Initialize();
        bool TryGetService(out object obj);
    }
}