﻿using UnityEngine;
using System.Collections;

public class ChangeSceneOnClick : MonoBehaviour
{

    public string sceneName;
    // public bool useZoomOnCick;
    //public Vector3 coordinateToZoom;


    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            Application.LoadLevel(sceneName);
        }
    }

    void Update()
    {
        /*  if (useZoomOnCick)
          {
              CameraMenu cm = new CameraMenu();
              cm.IsIsleClicked = true;
              //CameraMenu.AutoZoom(coordinateToZoom);
          }*/


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
