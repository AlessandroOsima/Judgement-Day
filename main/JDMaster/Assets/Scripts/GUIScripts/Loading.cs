using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Loading : MonoBehaviour
{

    string sceneName;
    string path = @"sceneName.dat";

    public void Start()
    {
		/*
        try
        {

            //Pass the file path and file name to the StreamReader constructor
            StreamReader sr = new StreamReader(path);

            //Read the first line of text
            sceneName = sr.ReadLine();

            //close the file
            sr.Close();
            Console.ReadLine();
        }
        catch (Exception e)
        {
            Debug.Log("Exception in stream reader: " + e.Message);
        }

		*/

        if (Application.GetStreamProgressForLevel(sceneName) == 1)
        {
			var GameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
			Application.LoadLevel(GameManager.NextScene);
        }
    }

}
