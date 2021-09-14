using JadesToolkit;
using System.Collections;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ServiceManager.StartService<SingletonExample>();
        }
    }

    private IEnumerator Timer(float t)
    {
        WaitForSeconds waitFor = new WaitForSeconds(t);
        ServiceManager.EndService<SingletonExample>();
        yield return waitFor;
        ServiceManager.StartService<SingletonExample2>();
        yield return waitFor;
        ServiceManager.ForceStartService<SingletonExample2>();
    }

}
