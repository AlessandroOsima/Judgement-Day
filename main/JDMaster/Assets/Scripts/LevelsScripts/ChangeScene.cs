using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class ChangeScene : MonoBehaviour
{


    public string sceneName;
    public Vector3 vectorZoom;

    string path = @"sceneName.dat";

    static TweenPosition tweenPos;

    void OnMouseDown()
    {



        if (Input.GetMouseButton(0))
        {

            tweenPos = CameraControlMenu.cameraTweenPosition;   //riprendo l'elemento TweenPosition della camera
            tweenPos.duration = 40f;
            tweenPos.ignoreTimeScale = true;
            tweenPos.method = UITweener.Method.EaseInOut;
            tweenPos.from = transform.position;
            tweenPos.to = vectorZoom;
            tweenPos.PlayForward();

            System.Threading.Thread.Sleep(200);


            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(path, false);

                //Write a line of text
                sw.WriteLine(sceneName);
                sw.Flush();

                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Debug.Log("Exception in WriteFile" + e.Message);
            }

            if (Application.GetStreamProgressForLevel("LoadingScreen") == 1)
            {
                Application.LoadLevel("LoadingScreen");
            }

        }
    }

}
