using JadesToolkit;
using UnityEngine;

[DoNotInitializOnLoad]
public class SingletonExample : ServiceBase<SingletonExample>
{
    public void DebugMyLog()
    {
        var x = ServiceManager.GetService<SingletonExample2>();
        x.DebugMyLog();
    }
}
