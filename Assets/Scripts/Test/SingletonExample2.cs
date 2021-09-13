using UnityEngine;
using JadesToolkit;
public class SingletonExample2 : SingletonBase<SingletonExample2>
{
    public void DebugMyLog()
    {
        Debug.Log(Instance.GetType());
    }
}
