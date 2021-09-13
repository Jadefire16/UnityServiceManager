using JadesToolkit;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ServiceManager.GetService<SingletonExample>().DebugMyLog();
        }
    }
}
