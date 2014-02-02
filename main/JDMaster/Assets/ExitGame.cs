using UnityEngine;
using System.Collections;

public class ExitGame : MonoBehaviour 
{
	bool quit = false;
	public float secondsUntilExit = 5000;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Mouse0))
			quit = true;

		if(Time.timeSinceLevelLoad >= secondsUntilExit)
			quit = true;

		if(quit)
		{
			Debug.Log("Quitting");
			Application.Quit();
		}
	}
}
