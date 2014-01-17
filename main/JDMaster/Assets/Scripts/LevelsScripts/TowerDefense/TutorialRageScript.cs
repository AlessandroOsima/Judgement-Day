using UnityEngine;
using System.Collections;

public class TutorialRageScript : MonoBehaviour 
{
	private static float height = Screen.height/10;
	private static float width = Screen.width/10;
	private static int charS = 13;
	private float time = 0f;
	private float dt;
	private int messageCount=0;
	private BasePowerDealer ragePower;
	private Vector3 originalCamPosition;
	private LevelGUI gui;
	private PersonStatus firstTze;
	private PersonStatus secondTze;
	// Use this for initialization
	void Start () 
	{
		originalCamPosition = Camera.main.transform.position;
		GlobalManager.globalManager.standardVictoryConditions = false;

		GlobalManager.globalManager.onEndGame += endGame;

		WriteMessage("Welcome to the next step almighty god!",0.75f,0.25f,7f);
		WriteMessage("This time we will take a look at some new cool powers !!",0.85f,0.25f,7f);


		ragePower = GlobalManager.globalManager.gameObject.transform.FindChild("Rage").GetComponent<BasePowerDealer>();
	}

	void WriteMessage(string s, float h,float w, float d)
	{
		float height = Screen.height/2;
		float width = Screen.width/2;
		
		Debug.Log(Screen.width);
		Debug.Log(Screen.height);
		
		int i = 0;
		int j = 0;
		float space = (w * width);
		string word;
		
		while ((j = s.IndexOf(" ", i)) != -1)
		{
			word = s.Substring(i,(j-i));
			LevelGUI.levelGUI.writeMessage(word, new Vector3(space, (h * height), 0f), new Vector3(1.2f,1.2f,1),d,true);
			space += (word.Length + 2) * charS;
			j++;
			i = j;
		}
		
		word = s.Substring(i);
		LevelGUI.levelGUI.writeMessage(word,new Vector3(space, (h * height), 0f), new Vector3(1.2f,1.2f,1),d,true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		dt=Time.deltaTime;

		time+=dt;

		LevelGUI.levelGUI.isDisplayingMessages();

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 0)
		{
			WriteMessage("But first take a look at them.",0.75f,0.25f,9f);
			WriteMessage("Have you noticed the red and blue balls hovering over their useless bodies ?",0.85f,0.25f,9f);	
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 1)
		{
			WriteMessage("This means that they are worshipping another god.",0.75f,0.25f,9f);
			WriteMessage("It's obvious that you must kill them.",0.85f,0.25f,9f);
			WriteMessage("Sadly, it's not gonna be so easy......",0.95f,0.25f,9f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 2)
		{
			WriteMessage("You cannot hurt the man marked by the blue sphere.",0.75f,0.25f,9f);
			WriteMessage("And if you kill the man with the red ball you will lose 6 souls.",0.85f,0.25f,9f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 3)
		{
			WriteMessage("But do not despair my vengeful friend.",0.75f,0.25f,10f);
			WriteMessage("You still can kill them using your indirect powers.",0.85f,0.25f,10f);
			WriteMessage("If you use one of this two powers (rage and zombie) on an unshielded human",0.95f,0.25f,10f);
			WriteMessage("he will try to kill all the nearest humans",1.05f,0.25f,10f);

			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 4)
		{
			WriteMessage("Shielded ones included.......",0.75f,0.25f,6f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 5)
		{
			WriteMessage("Now, these are the powers at your disposal :",0.75f,0.25f,10f);
			WriteMessage("Zombie will create a shiny new zombie.",0.85f,0.25f,10f);
			WriteMessage("Remember though that zombie decay and die very fast",0.95f,0.25f,10f);
			WriteMessage("Rage will transform an human into the perfect serial killer.",1.05f,0.25f,10f);
			WriteMessage("Calm removes rage and stops a character for sometime.",1.15f,0.25f,10f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 6)
		{
			GlobalManager.globalManager.incrementSouls(10);
			WriteMessage("Ok, I just gave you 10 souls, use them as you wish.",0.75f,0.25f,9f);
			WriteMessage("To win this battle you have to kill everyone except the red shielded man.",0.85f,0.25f,9f);
			WriteMessage("If you lose all your souls you will die.",1.05f,0.25f,9f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 7)
		{
			WriteMessage("Enough chit chat, let's get this genocide started",0.75f,0.25f,7f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 8 && GlobalManager.globalManager.population == 0)
		{
			WriteMessage("You have killed them all",0.75f,0.25f,9f);
			WriteMessage("Very good, I think you'll like what's coming next",0.85f,0.25f,9f);
			messageCount++;
		}




		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 9)
		{
			Application.LoadLevel("Sandbox");
		}
	}

	void endGame(EndGameState endGameState)
	{
		if(endGameState == EndGameState.Defeat)
		{
			WriteMessage("Looks like you are going to have to try again",0.75f,0.25f,9f);
			WriteMessage("Just press the Redo button up in the left",0.85f,0.25f,0f);
		}
		else
		{
			WriteMessage("You have killed them all",0.75f,0.25f,9f);
			WriteMessage("Very good, I think you'll like what's coming next",0.85f,0.25f,9f);
			messageCount++;
		}
		

	}
}