using UnityEngine;
using System.Collections;

public class TutorialRageScript : MonoBehaviour 
{
	private static int charS = 13;
	private float time = 0f;
	private float dt;
	private int messageCount=0;
	private BasePowerDealer ragePower;
	private Vector3 originalCamPosition;
	private LevelGUI gui;
	private PersonStatus firstTze;
	private PersonStatus secondTze;
	private PersonStatus thirdTze;
	public float messagePass=0.5f;

	// Use this for initialization
	void Start () 
	{
		originalCamPosition = Camera.main.transform.position;
		GlobalManager.globalManager.standardVictoryConditions = false;
		var persons = GameObject.FindGameObjectsWithTag("Person");

		firstTze = persons[0].GetComponent<PersonStatus>();
		secondTze = persons[1].GetComponent<PersonStatus>();
		thirdTze = persons[2].GetComponent<PersonStatus>();

		GlobalManager.globalManager.onEndGame += endGame;

		WriteMessage("Welcome to the next step almighty god!",1.7f,0.25f,0f);
		WriteMessage("This time we will take a look at some new cool powers !!",1.8f,0.25f,0f);


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
				WriteMessage("But first take a look at the people.",1.7f,0.25f,9f);
				WriteMessage("Have you noticed the red and blue balls hovering over their useless bodies ?",1.8f,0.25f,9f);	
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
				WriteMessage("This means that they are worshipping another god.",1.7f,0.25f,9f);
				WriteMessage("It is obvious that you must kill them, but is not gonna be so easy",1.8f,0.25f,9f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 2  )
		{	
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("You cannot hurt the man marked by the blue sphere.",1.7f,0.25f,9f);
				WriteMessage("And if you kill the man with the red ball you will lose 6 souls.",1.8f,0.25f,9f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 3  )
		{	
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("You still can kill them using your indirect powers.",1.7f,0.25f,9f);
				WriteMessage("With Rage you can use a human to kill others without killing him.",1.8f,0.25f,9f);
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
				ragePower.powerState = PowerState.Disabled;
				PowersManager.powersManager.refreshPowersStates(GlobalManager.globalManager.souls);
				((BasePowerDealer)ragePower).enableUse = false;
				WriteMessage("Press Number 4 to select RAGE!",1.7f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}
		
		

		if((Input.GetKey(KeyCode.Keypad4) || Input.GetKey("4")) && messageCount == 5 
		   && PowersManager.powersManager.SelectedPower == ragePower)
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				((BasePowerDealer)ragePower).enableUse = true;
				WriteMessage("Target the one with the red orb",1.7f,0.25f,0f);
				WriteMessage("Press SPACE to use your Power.",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if(messageCount == 6 && GlobalManager.globalManager.population <= 2){
			messageCount++;
		}
		if(messageCount == 7)
		{
			if(LevelGUI.levelGUI.isDisplayingMessages()){
				LevelGUI.levelGUI.clearMessages();
			}
			WriteMessage("Perfect!! Let the RAGE flow through you",1.7f,0.25f,0f);
			messageCount++;
		}

		if(messageCount==20 && GlobalManager.globalManager.population >= 1 && GlobalManager.globalManager.souls<=5)
		{
			if(time>=10f){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("Do not worry. You can Redo the level any time",1.7f,0.25f,0f);
				WriteMessage("Press the Redo button up in the left",1.8f,0.25f,0f);
				messageCount++;
			}
		}
		
		if(messageCount == 6 && GlobalManager.globalManager.souls < 5 && GlobalManager.globalManager.population > 2 && firstTze.ActivePower == null && secondTze.ActivePower == null)
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
				WriteMessage("Now, to the other powers",1.7f,0.25f,0f);
				WriteMessage("Lightning (2) will kill inmediately with an electric shock",1.8f,0.25f,10f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 9 )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("ZOMBIE (3) will create a shiny new zombie.",1.7f,0.25f,0f);
				WriteMessage("Be aware though that zombie decay and die very fast with no food",1.8f,0.25f,10f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 10 )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				WriteMessage("CALM (5) make people stay still.",1.7f,0.25f,0f);
				WriteMessage("It is a support power, it takes Rage away",1.8f,0.25f,10f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 11 )
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				GlobalManager.globalManager.incrementSouls(10);
				WriteMessage("Ok, now you got 10 extra souls.",1.7f,0.25f,0f);
				WriteMessage("Judge everyone, will ya",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if(messageCount == 12 && GlobalManager.globalManager.population == 0)
		{
			if(time>=messagePass){
				if(LevelGUI.levelGUI.isDisplayingMessages()){
					LevelGUI.levelGUI.clearMessages();
				}
				GlobalManager.globalManager.incrementSouls(10);
				WriteMessage("You have killed them all",1.7f,0.25f,0f);
				WriteMessage("Very good, I think you'll like what's coming next",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 13 )
		{
			Application.LoadLevel("City");
		}
	}

	void endGame(EndGameState endGameState)
	{
		if(endGameState == EndGameState.Defeat)
		{
			if(LevelGUI.levelGUI.isDisplayingMessages()){
				LevelGUI.levelGUI.clearMessages();
			}
			WriteMessage("Looks like you are going to have to try again",1.7f,0.25f,0f);
			WriteMessage("Just press the Redo button up in the left",1.8f,0.25f,0f);
		}
		else
		{
			if(LevelGUI.levelGUI.isDisplayingMessages()){
				LevelGUI.levelGUI.clearMessages();
			}
			WriteMessage("You have killed them all",1.7f,0.25f,0f);
			WriteMessage("Very good, I think you'll like what's coming next",1.8f,0.25f,0f);
			messageCount++;
		}
		

	}
}