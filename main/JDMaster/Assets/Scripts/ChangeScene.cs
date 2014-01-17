using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour
{

    public string sceneName;

    void Update()
    {
        if (Input.GetMouseButton(0))
            try
            {
                Application.LoadLevel(sceneName);
            }
            catch
            {
                Debug.LogError("Scene does NOT exist!");
            }
    }
}
