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
			LevelGUI.levelGUI.writeWord("DEFEAT ",new Vector3(Screen.width/2, Screen.height/2,0f), new Vector3(1.4f,1.4f,1),0f,false);
			reload = true;
		}
		else
		{
			Debug.Log(endGameState);
			LevelGUI.levelGUI.writeWord("VICTORY",new Vector3(Screen.width/2, Screen.height/2,0f), new Vector3(1.4f,1.4f,1),0f,false);
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
