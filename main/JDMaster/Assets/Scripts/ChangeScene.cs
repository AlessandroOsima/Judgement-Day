using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour
{

    public string sceneName;
   // public bool useZoomOnCick;
    //public Vector3 coordinateToZoom;

    
    void Update()
    {
      /*  if (useZoomOnCick)
        {
            CameraMenu cm = new CameraMenu();
            cm.IsIsleClicked = true;
            //CameraMenu.AutoZoom(coordinateToZoom);
        }*/
        
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
    /*
    #region getter setter
    public Vector3 CoordinatesToZoom
    {
        get
        {
            return coordinateToZoom;
        }
    }
    #endregion
    */
    
}
