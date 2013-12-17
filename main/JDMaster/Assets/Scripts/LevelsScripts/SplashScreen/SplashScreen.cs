using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public UIToolkit SplashScreenToolkit;
	UISprite JDLogo;
	float timer = 0;

	// Use this for initialization
	void Start () 
	{
		JDLogo = SplashScreenToolkit.addSprite("JD_Logo.png",0,0);
		JDLogo.positionCenter();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.Space))
			Application.LoadLevel("TowerDefense");

		if(Input.GetKey(KeyCode.KeypadEnter))
			Application.LoadLevel("TowerDefense");
	
		if(Input.GetKey(KeyCode.Alpha0))
			Application.LoadLevel("TowerDefense");

		if(Input.GetKey(KeyCode.Alpha1))
			Application.LoadLevel("Sandbox");
	}
}
