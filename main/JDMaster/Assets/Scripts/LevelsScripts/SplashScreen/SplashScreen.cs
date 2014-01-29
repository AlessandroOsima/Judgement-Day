using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	public UIToolkit SplashScreenToolkit;
	UISprite JDLogo;
	float timer = 0;
	bool started = false;
	bool artskillLogo = false;

	// Use this for initialization
	void Start () 
	{
		JDLogo = SplashScreenToolkit.addSprite("PolimiGameCollective.png",0,0);
		JDLogo.positionCenter();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.timeSinceLevelLoad > 12.0f && !started)
		{
			JDLogo.destroy();
			Camera.main.backgroundColor = Color.white;
			JDLogo = SplashScreenToolkit.addSprite("JD_Logo.png",0,0);
			JDLogo.positionCenter();
			Camera.main.audio.Play();
			started = true;
		}

		if(Time.timeSinceLevelLoad > 6.0f && Time.timeSinceLevelLoad < 12.0f && !artskillLogo)
		{
			JDLogo.destroy();
			Camera.main.backgroundColor = Color.white;
			JDLogo = SplashScreenToolkit.addSprite("artskillz_logo.png",0,0);
			JDLogo.positionCenter();
			artskillLogo = true;
		}
	
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

		if(Input.GetKey(KeyCode.Alpha6))
			Application.LoadLevel("EasterIsland");

		if(!Camera.main.audio.isPlaying && started)
			Application.LoadLevel("LevelSelectionMenu");
	}
}
