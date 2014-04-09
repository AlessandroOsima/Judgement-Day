using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

public class ChangeScene : MonoBehaviour
{


    public string sceneName;
    public float zoomDuration = 4f;
    public Vector3 vectorZoom;

    string path = @"sceneName.dat";
    static TweenPosition tweenPos;
    bool running = true;



    private List<Vector3> islandsPositions;
    private int selectedIsland = 0;

    void OnMouseDown()
    {



        if (Input.GetMouseButton(0))
        {


            islandsPositions = CameraControlMenu.IslandsPositions;
            selectedIsland = CameraControlMenu.SelectedIsle;
            tweenPos = CameraControlMenu.cameraTweenPosition;   //riprendo l'elemento TweenPosition della camera
            tweenPos.duration = zoomDuration;
            tweenPos.ignoreTimeScale = true;
            tweenPos.method = UITweener.Method.EaseIn;
            tweenPos.from = islandsPositions[selectedIsland];
            tweenPos.to = vectorZoom;
            tweenPos.ResetToBeginning();
            tweenPos.onFinished.Clear();
            EventDelegate.Add(tweenPos.onFinished, OnTweenFinished);
            tweenPos.PlayForward();

        }
    }

    void Update()
    {
        if (running) return;


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


    private void OnTweenFinished()
    {
        running = false;
    }

}
