using JadesToolkit;
using UnityEngine;

public class SingletonExample : SingletonBase<SingletonExample>
{
    public void DebugMyLog()
    {
        Debug.Log(Instance.GetType());
    }
}
