using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour 
{

	public UISprite foregroundSpritePolimi;
	public UISprite foregroundSpriteJD;
	public UITexture backgrounSprite;
	public Camera uiCamera;
	float timer = 0;
	bool started = false;


	// Use this for initialization
	void Start () 
	{
		//this.GetComponent<UIRoot>().manualHeight = Screen.height;
	}
	
	// Update is called once per frame
	void Update () 
	{
		foregroundSpriteJD.ResetAnchors();
		foregroundSpritePolimi.ResetAnchors();
		foregroundSpriteJD.UpdateAnchors();
		foregroundSpritePolimi.UpdateAnchors();

		if(Time.timeSinceLevelLoad > 6.0f && !started)
		{
			backgrounSprite.color = Color.white;
			foregroundSpritePolimi.alpha = 0f;
			foregroundSpriteJD.alpha = 1f;
			uiCamera.audio.Play();
			started = true;
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

		if(!uiCamera.audio.isPlaying && started)
			Application.LoadLevel("LevelSelectionMenu");
	}
}
