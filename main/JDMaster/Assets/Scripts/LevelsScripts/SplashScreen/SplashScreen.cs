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
	
		if(InputMapping.GetAction(Actions.Skip) > 0)
			Application.LoadLevel("LevelSelectionMenu");

        if (Input.GetMouseButtonDown(0))
            Application.LoadLevel("LevelSelectionMenu");

		if(!uiCamera.audio.isPlaying && started)
			Application.LoadLevel("LevelSelectionMenu");
	}
}
