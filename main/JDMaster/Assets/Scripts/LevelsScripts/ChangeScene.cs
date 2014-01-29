using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class ChangeScene : MonoBehaviour
{
	
	public string sceneName;
	string path = @"sceneName.dat";
	
	void OnMouseDown()
	{
		if (Input.GetMouseButton(0))
		{
			
			
			
			
			
			try
			{
				
				//Pass the filepath and filename to the StreamWriter Constructor
				StreamWriter sw = new StreamWriter(path,false);
				
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