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
	CameraControlMenu menuCamera;
	
	
	
	private List<GameObject> islandsPositions;
	private int selectedIsland = 0;

	void Start()
	{
		menuCamera = FindObjectOfType<CameraControlMenu>();
		tweenPos = CameraControlMenu.cameraTweenPosition;   //riprendo l'elemento TweenPosition della camera
	}
	
	void OnMouseDown()
	{
		if (Input.GetMouseButton(0) && !menuCamera.isRunning)
		{
			menuCamera.isRunning = true;
			islandsPositions = CameraControlMenu.IslandsPositions;
			selectedIsland = menuCamera.SelectedIsle;
			tweenPos.duration = zoomDuration;
			tweenPos.ignoreTimeScale = true;
			tweenPos.method = UITweener.Method.EaseIn;
			tweenPos.from = menuCamera.transform.position;
			tweenPos.to = new Vector3(islandsPositions[selectedIsland].transform.position.x,25,islandsPositions[selectedIsland].transform.position.z);
			tweenPos.ResetToBeginning();
			tweenPos.onFinished.Clear();
			EventDelegate.Add(tweenPos.onFinished, OnTweenFinished);
			tweenPos.PlayForward();
			
		}
	}

	void Update()
	{
		if (running) 
			return;
		
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
