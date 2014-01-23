using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour 
{
	private static int charS = 13;
	private float time = 0f;
	private float dt;
	private int messageCount=0;
	private Power firePower;
	private Vector3 originalCamPosition;
	private LevelGUI gui;
	private PersonStatus firstTze;
	private PersonStatus secondTze;
	public float messagePass=0.5f;

	// Use this for initialization
	void Start () 
	{

		originalCamPosition = Camera.main.transform.position;
		var persons = GameObject.FindGameObjectsWithTag("Person");
		firstTze = persons[0].GetComponent<PersonStatus>();
		secondTze = persons[1].GetComponent<PersonStatus>();
		GlobalManager.globalManager.standardVictoryConditions = false;

		WriteMessage("Welcome to Judgement Day!",1.7f,0.25f,0f);
		WriteMessage("Press Enter to Continue",1.8f,0.25f,0f);

		firePower = GlobalManager.globalManager.gameObject.transform.FindChild("Fire").GetComponent<BasePowerDealer>();
	}

	void WriteMessage(string s, float h,float w, float d)
	{
		float height = Screen.height/2;
		float width = Screen.width/2;

		int i = 0;
		int j = 0;
		float space = (w * width);
		string word;
		
		while ((j = s.IndexOf(" ", i)) != -1)
		{
			word = s.Substring(i,(j-i));
			LevelGUI.levelGUI.writeWord(word, new Vector3(space, (h * height), 0f), new Vector3(1.2f,1.2f,1),d,true);
			space += (word.Length + 2) * charS;
			j++;
			i = j;
		}
		
		word = s.Substring(i);
		LevelGUI.levelGUI.writeWord(word,new Vector3(space, (h * height), 0f), new Vector3(1.2f,1.2f,1),d,true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		dt=Time.deltaTime;

		time+=dt;

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 0   )
		{	
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("You are a GOD!!!",1.7f,0.25f,0f);
				WriteMessage("but these people are not adoring you",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 1   )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("Look at them just laughing.",1.7f,0.25f,0f);
				WriteMessage("To zoom press Q.",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey("q")) && messageCount == 2  )
		{
			if(time>=messagePass)
			{
				if(LevelGUI.levelGUI.isDisplayingMessages())
				{
					LevelGUI.levelGUI.clearMessages();
				}

				WriteMessage("Good, you can zoom back with E.",1.7f,0.25f,0f);
				WriteMessage("You should use your powers to teach them who's the boss.",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}


		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey("e")) && messageCount == 3  )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("Ok, let's try a little FIRE!!",1.7f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}


		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 4  )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				firePower.powerState = PowerState.Disabled;
				PowersManager.powersManager.refreshPowersStates(GlobalManager.globalManager.souls);
				((BasePowerDealer)firePower).enableUse = false;
				WriteMessage("Press Number 1 to select FIRE!",1.7f,0.25f,0f);
				WriteMessage("Use the arrows to move the god ray to your target.",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}



		if((Input.GetKey(KeyCode.Keypad1) || Input.GetKey("1")) && messageCount == 5 
		   							&& PowersManager.powersManager.SelectedPower == firePower)
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				((BasePowerDealer)firePower).enableUse = true;
				WriteMessage("Good! Now burn the unworthy.",1.7f,0.25f,0f);
				WriteMessage("Press SPACE to use your Power.",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}
		if(messageCount == 6 && GlobalManager.globalManager.population <= 1){
			messageCount++;
		}
		if(messageCount == 7)
		{
			if(LevelGUI.levelGUI.isDisplayingMessages()){
				LevelGUI.levelGUI.clearMessages();
			}
			WriteMessage("HAHAHAHAHHAHAHAHAHAHAHA...",1.7f,0.25f,0f);
			messageCount++;
		}

		if(messageCount == 6 && GlobalManager.globalManager.souls < 5 && GlobalManager.globalManager.population > 1 && firstTze.ActivePower == null && secondTze.ActivePower == null)
		{
			if(LevelGUI.levelGUI.isDisplayingMessages()){
				LevelGUI.levelGUI.clearMessages();
			}
			WriteMessage("You missed. Did you not?",1.7f,0.25f,0f);
			messageCount = 20;
			time=0;
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 8  )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("Sorry, I got carried away",1.7f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if(messageCount==20 && GlobalManager.globalManager.population >= 1 && GlobalManager.globalManager.souls<=7)
		{
			if(time>=5f){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("Do not worry. You can Redo the level any time",1.7f,0.25f,0f);
				WriteMessage("Press the Redo button up in the left",1.8f,0.25f,0f);
				messageCount++;
			}
		}


		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 9)
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				((BasePowerDealer)firePower).enableUse = false;
				WriteMessage("However be aware! When you use your powers you use SOULS",1.7f,0.25f,0f);
				WriteMessage("The number of SOULS you own is up there with the crosses",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 10  )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("You have a limited number of SOULS",1.7f,0.25f,0f);
				WriteMessage("But with each infidel dead you get his sins weight in SOULS",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 11  )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("Some will be worth more of your time, some less",1.7f,0.25f,0f);
				WriteMessage("It depends on their actions, and Yours. These guys worth 3 souls each",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 12  )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("The cost of the Powers is stated next to it",1.7f,0.25f,0f);
				WriteMessage("You can use some extra SOULS this time",1.8f,0.25f,0f);
				GlobalManager.globalManager.incrementSouls(8);
				messageCount++;
				time=0;
			}
		}
	
		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 13  )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				if(GlobalManager.globalManager.population > 0)
				{
					((BasePowerDealer)firePower).enableUse = true;
					WriteMessage("Now, Do me a favor and kill that other infidel too.",1.7f,0.25f,0f);
					messageCount++;
				}
				else
				{
					WriteMessage("Nice!! you got 2 for 1",1.7f,0.25f,0f);
					messageCount+=2;
				}
				time=0;
			}
		}

		if(GlobalManager.globalManager.population == 0 && messageCount == 14  )
		{	
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("Good Job!!!",1.7f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if(messageCount == 14  && GlobalManager.globalManager.population > 0 && GlobalManager.globalManager.souls < 5)
		{	
			if(time>=5f){
				messageCount=20;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 15  )
		{	
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("Great Work !!! You managed to kill all the infidels.",1.7f,0.25f,0f);
				WriteMessage("Now, let's go kill some more people",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}


		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 16  )
		{	
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
			Application.LoadLevel("TutorialRage");
			}
		}
	}
}