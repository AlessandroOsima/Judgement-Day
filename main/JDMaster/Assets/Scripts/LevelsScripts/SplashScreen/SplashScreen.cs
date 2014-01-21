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
			Application.LoadLevel("LevelSelectionMenu");

		if(Input.GetKey(KeyCode.KeypadEnter))
			Application.LoadLevel("LevelSelectionMenu");

		if(Input.GetKey(KeyCode.Alpha1))
			Application.LoadLevel("TutorialFire");

		if(Input.GetKey(KeyCode.Alpha2))
			Application.LoadLevel("TutorialRage");
	
		if(Input.GetKey(KeyCode.Alpha3))
			Application.LoadLevel("City");

		if(Input.GetKey(KeyCode.Alpha4))
			Application.LoadLevel("Stonehenge");

		if(Input.GetKey(KeyCode.Alpha5))
			Application.LoadLevel("TowerDefense");
	}
}
