using JadesToolkit;
using UnityEngine;

public class SingletonExample : ServiceBase<SingletonExample>
{
    public void DebugMyLog()
    {
        ServiceManager.GetService<SingletonExample2>().DebugMyLog();
    }
}
