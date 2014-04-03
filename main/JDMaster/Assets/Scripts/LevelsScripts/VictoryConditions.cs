using UnityEngine;
using System.Collections;

public class VictoryConditions : MonoBehaviour {

	public string nextLevel = "Stonehenge";
	bool reload;
	bool startCounting = false;
	float timer = 0;

	// Use this for initialization
	void Start () 
	{
		GlobalManager.globalManager.onEndGame += endGame;
	}

	void endGame(EndGameState endGameState)
	{
		if(endGameState == EndGameState.Defeat)
		{
			Debug.Log(endGameState);
			LevelGUI.levelGUI.WriteMessage("GAME OVER ",Screen.width/2f, Screen.height/2f,0f,LevelGUI.sRed,true,80);
			GameObject.Find("RTSCameraSoundtrack").GetComponent<CameraControl>().enabled = false;
			reload = true;
		}
		else
		{
			Debug.Log(endGameState);
			LevelGUI.levelGUI.WriteMessage("YOU ARE A GOD !!",Screen.width/2f, Screen.height/2f,0f,LevelGUI.sBlue,true,80);
			GameObject.Find("RTSCameraSoundtrack").GetComponent<CameraControl>().enabled = false;
			reload = false;
		}

		startCounting = true;
	}

	// Update is called once per frame
	void Update () 
	{
		if(startCounting)
		{
			timer += Time.deltaTime;
			if(timer >= 10f)
			{
				if(reload)
					Application.LoadLevel(Application.loadedLevelName);
				else
					Application.LoadLevel(nextLevel);
			}
		}
	}
}
