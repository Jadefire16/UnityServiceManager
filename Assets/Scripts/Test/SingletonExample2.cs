using UnityEngine;
using JadesToolkit;
using System.Collections;

public class SingletonExample2 : ServiceBase<SingletonExample2>
{
    public void DebugMyLog()
    {
        Debug.Log("Hello");
        ServiceManager.TryEndService<SingletonExample>();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3f);
        ServiceManager.StartService<SingletonExample>();
    }
}
