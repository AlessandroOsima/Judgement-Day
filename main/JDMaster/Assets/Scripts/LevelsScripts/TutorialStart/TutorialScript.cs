using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour 
{
	private static int charS = 14;
	private float time = 0f;
	private float dt;
	private int messageCount=0;
	private Power firePower;
	private Vector3 originalCamPosition;
	private LevelGUI gui;
	private PersonStatus firstTze;
	private PersonStatus secondTze;

	// Use this for initialization
	void Start () 
	{

		originalCamPosition = Camera.main.transform.position;
		var persons = GameObject.FindGameObjectsWithTag("Person");
		firstTze = persons[0].GetComponent<PersonStatus>();
		secondTze = persons[1].GetComponent<PersonStatus>();
		GlobalManager.globalManager.standardVictoryConditions = false;

		WriteMessage("Welcome to Judgement Day!",0.75f,0.25f,6f);

		firePower = GlobalManager.globalManager.gameObject.transform.FindChild("Fire").GetComponent<BasePowerDealer>();
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
			WriteMessage("You are a GOD!!!",0.75f,0.25f,6f);
			WriteMessage("but these people are not adoring you",0.85f,0.25f,6f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==1)
		{
			WriteMessage("Look at them just laughing.",0.75f,0.25f,5f);
			WriteMessage("To zoom press Q.",0.85f,0.25f,5f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 2 && Camera.main.transform.position.y < originalCamPosition.y)
		{
			WriteMessage("Good, you can zoom back with E.",0.75f,0.25f,7f);
			WriteMessage("You should use your powers to teach them who's the boss.",0.85f,0.25f,7f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==3)
		{
			WriteMessage("Let's try a little FIRE!!",0.75f,0.25f,5f);

			firePower.powerState = PowerState.Disabled;
			PowersManager.powersManager.refreshPowersStates(GlobalManager.globalManager.souls);
			((BasePowerDealer)firePower).enableUse = false;

			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==4)
		{
			WriteMessage("Press 1 to select FIRE!",0.75f,0.25f,8f);
			WriteMessage("Use the arrows to move the god ray to your target.",0.85f,0.25f,8f);
			messageCount = 6;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==6 && PowersManager.powersManager.SelectedPower == firePower)
		{
			messageCount = 8;
			((BasePowerDealer)firePower).enableUse = true;
			WriteMessage("Good! Now burn the unworthy.",0.75f,0.25f,6f);
			WriteMessage("Press SPACE to use your Power.",0.85f,0.25f,6f);
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 8 && GlobalManager.globalManager.population <= 1)
		{
			WriteMessage("JAJAJAJAJAJAJAJAJA...",0.75f,0.25f,4f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() &&  messageCount == 8 && GlobalManager.globalManager.souls < 5 && GlobalManager.globalManager.population > 1 && firstTze.ActivePower == null && secondTze.ActivePower == null)
		{
			WriteMessage("You missed. Did you not?",0.75f,0.25f,6f);
			messageCount = 9;
		}
		
		
		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==9 && GlobalManager.globalManager.population < 2){
			WriteMessage("Sorry, I got carried away",0.75f,0.25f,6f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==9 && GlobalManager.globalManager.population == 2 || (!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==16 && GlobalManager.globalManager.souls<=7 && GlobalManager.globalManager.population > 0))
		{
			WriteMessage("Do not worry. You can Redo the level any time",0.75f,0.25f,6f);
			WriteMessage("Press the Redo button up in the left",0.85f,0.25f,0f);
			messageCount+=10;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 10)
		{
			((BasePowerDealer)firePower).enableUse = false;
			WriteMessage("However be aware! When you use your powers you use SOULS",0.75f,0.25f,8f);
			WriteMessage("The number of SOULS you own is up there with the crosses",0.85f,0.25f,8f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==11){
			WriteMessage("You have a limited number of SOULS",0.75f,0.25f,8f);
			WriteMessage("But with each infidel dead you get his sins weight in SOULS",0.85f,0.25f,8f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==12)
		{
			WriteMessage("Some will be worth more of your time, some less",0.75f,0.25f,8f);
			WriteMessage("It depends on their actions, and Yours",0.85f,0.25f,8f);
			WriteMessage("This guys will give you 3 souls each",0.95f,0.25f,8f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==13){
			WriteMessage("The cost of the Powers is stated next to it",0.75f,0.25f,8f);
			WriteMessage("You can use some extra SOULS this time",0.85f,0.25f,8f);
			GlobalManager.globalManager.incrementSouls(8);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==14)
		{
			if(GlobalManager.globalManager.population > 0)
			{
				((BasePowerDealer)firePower).enableUse = true;
				WriteMessage("Now, Do me a favor and kill that other infidel too.",0.75f,0.25f,7f);
				messageCount++;
			}
			else
			{
				WriteMessage("Nice!! you got 2 for 1",0.75f,0.25f,8f);
				messageCount+=2;
			}
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 15 && GlobalManager.globalManager.population == 0)
		{
			WriteMessage("Good Job!!!",0.75f,0.25f,4f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount == 15 && GlobalManager.globalManager.population > 0 && GlobalManager.globalManager.souls < 5)
		{
			WriteMessage("Looks like you are out of souls. Do not worry, you can Redo the level any time",0.75f,0.25f,7f);
			WriteMessage("Just press the Redo button up in the left",0.85f,0.25f,0f);
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==16 && GlobalManager.globalManager.population <= 0)
		{
			WriteMessage("Great Work !!! You managed to kill all the infidels.",0.75f,0.25f,10f);
			WriteMessage("Now, let's go kill some more people",0.85f,0.25f,10f);
			messageCount++;
		}

		if(!LevelGUI.levelGUI.isDisplayingMessages() && messageCount==17 && GlobalManager.globalManager.population <= 0)
		{
			Application.LoadLevel("TutorialRage");
		}
	}
}