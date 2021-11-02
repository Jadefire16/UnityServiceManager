using JadesToolkit.Services.Interfaces;
using UnityEngine;

public class Test : IServiceBehaviour
{
    public void Initialize()
    {
        Debug.Log("Initialized");
    }

    public bool TryGetService(out object obj)
    {
        obj = null;
        return true;
    }
}
