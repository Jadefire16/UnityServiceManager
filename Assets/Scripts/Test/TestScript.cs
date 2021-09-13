using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SingletonExample.Instance.DebugMyLog();
            SingletonExample2.Instance.DebugMyLog();
        }
    }
}
