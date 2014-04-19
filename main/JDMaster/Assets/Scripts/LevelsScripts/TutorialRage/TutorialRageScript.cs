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
	private UILabel tutorialLabel;

	// Use this for initialization
	void Start () 
	{
		tutorialLabel = this.GetComponent<UILabel>();
		originalCamPosition = Camera.main.transform.position;
		GlobalManager.globalManager.standardVictoryConditions = false;
		var persons = GameObject.FindGameObjectsWithTag("Person");

		firstTze = persons[0].GetComponent<PersonStatus>();
		secondTze = persons[1].GetComponent<PersonStatus>();
		thirdTze = persons[2].GetComponent<PersonStatus>();

		GlobalManager.globalManager.onEndGame += endGame;


		tutorialLabel.text = "Welcome to the next step almighty god! This time we will take a look at some new cool powers !!";

		//WriteMessage("Welcome to the next step almighty god!",1.7f,0.25f,0f);
		//WriteMessage("This time we will take a look at some new cool powers !!",1.8f,0.25f,0f);


		ragePower = GlobalManager.globalManager.gameObject.transform.FindChild("Rage").GetComponent<BasePowerDealer>();

	}

	// Update is called once per frame
	void Update () 
	{
		dt = Time.deltaTime;

		time += dt;

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 0   )
		{	
			if(time>=messagePass)
			{
				tutorialLabel.text = "But first take a look at the people. Have you noticed the red and blue balls hovering over their bodies ?";

				//WriteMessage("But first take a look at the people.",1.7f,0.25f,9f);
				//WriteMessage("Have you noticed the red and blue balls hovering over their useless bodies ?",1.8f,0.25f,9f);	
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 1   )
		{	
			if(time>=messagePass)
			{
				tutorialLabel.text = "This means that they are worshipping another god. It is obvious that you must kill them, but is not gonna be so easy.";

				//WriteMessage("This means that they are worshipping another god.",1.7f,0.25f,9f);
				//WriteMessage("It is obvious that you must kill them, but is not gonna be so easy",1.8f,0.25f,9f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 2  )
		{	
			if(time>=messagePass)
			{
				tutorialLabel.text = "You cannot hurt the man marked by the blue sphere. And if you kill the man with the red ball you will lose 6 souls.";

				//WriteMessage("You cannot hurt the man marked by the blue sphere.",1.7f,0.25f,9f);
				//WriteMessage("And if you kill the man with the red ball you will lose 6 souls.",1.8f,0.25f,9f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 3  )
		{	
			if(time>=messagePass)
			{
				tutorialLabel.text = "You still can kill them using your indirect powers. With Rage you can use a human to kill others without killing him.";

				//WriteMessage("You still can kill them using your indirect powers.",1.7f,0.25f,9f);
				//WriteMessage("With Rage you can use a human to kill others without killing him.",1.8f,0.25f,9f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 4  )
		{
			if(time>=messagePass)
			{
				GlobalManager.globalManager.incrementSouls(10);

				ragePower.powerState = PowerState.Disabled;
				PowersManager.powersManager.refreshPowersStates(GlobalManager.globalManager.souls);
				((BasePowerDealer)ragePower).enableUse = false;

				tutorialLabel.text = "Press Number 4 to select RAGE!";

				//WriteMessage("Press Number 4 to select RAGE!",1.7f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}
		
		

		if((Input.GetKey(KeyCode.Keypad4) || Input.GetKey("4")) && messageCount == 5 && PowersManager.powersManager.SelectedPower == ragePower)
		{
			if(time>=messagePass)
			{

				((BasePowerDealer)ragePower).enableUse = true;

				tutorialLabel.text = "Target the one with the red orb. Press SPACE to use your Power.";

				//WriteMessage("Target the one with the red orb",1.7f,0.25f,0f);
				//WriteMessage("Press SPACE to use your Power.",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if(messageCount == 6 && GlobalManager.globalManager.population <= 2)
		{
			messageCount++;
		}

		if(messageCount == 7)
		{
			tutorialLabel.text = "Perfect!! Let the RAGE flow through you";

			//WriteMessage("Perfect!! Let the RAGE flow through you",1.7f,0.25f,0f);
			messageCount++;
		}

		if(messageCount==20 && GlobalManager.globalManager.population >= 1 && GlobalManager.globalManager.souls<=5)
		{
			if(time>=10f)
			{
				tutorialLabel.text = "Do not worry. You can Redo the level any time. Press the Redo button up in the left.";

				//WriteMessage("Do not worry. You can Redo the level any time",1.7f,0.25f,0f);
				//WriteMessage("Press the Redo button up in the left",1.8f,0.25f,0f);
				messageCount++;
			}
		}
		
		if(messageCount == 6 && GlobalManager.globalManager.souls < 5 && GlobalManager.globalManager.population > 2 && firstTze.ActivePower == null && secondTze.ActivePower == null)
		{
			tutorialLabel.text = "You missed. Did you not?";

			//WriteMessage("You missed. Did you not?",1.7f,0.25f,0f);
			messageCount = 20;
			time=0;
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 8  )
		{
			if(time>=messagePass)
			{
				tutorialLabel.text = "Now, to the other powers. Lightning (1) will kill immediately with an electric shock.";

				//WriteMessage("Now, to the other powers",1.7f,0.25f,0f);
				//WriteMessage("Lightning (2) will kill inmediately with an electric shock",1.8f,0.25f,10f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 9 )
		{
			if(time>=messagePass)
			{
				tutorialLabel.text = "CALM (2) make people stay still. It is a support power, it takes Rage away.";

				//WriteMessage("ZOMBIE (3) will create a shiny new zombie.",1.7f,0.25f,0f);
				//WriteMessage("Be aware though that zombie decay and die very fast with no food",1.8f,0.25f,10f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 10 )
		{
			if(time>=messagePass)
			{
				tutorialLabel.text = "ZOMBIE (3) will create a shiny new zombie. Be aware though that zombie decay and die very fast with no food";

				//WriteMessage("CALM (5) make people stay still.",1.7f,0.25f,0f);
				//WriteMessage("It is a support power, it takes Rage away",1.8f,0.25f,10f);
				messageCount++;
				time=0;
			}
		}

		if((Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Return)) && messageCount == 11 )
		{
			if(time>=messagePass)
			{
				GlobalManager.globalManager.incrementSouls(10);

				foreach(var power in PowersManager.powersManager.powers)
				{
					power.powerState = PowerState.Disabled;

				}

				PowersManager.powersManager.refreshPowersStates(GlobalManager.globalManager.souls);

				tutorialLabel.text = "Ok, now you got 10 extra souls. Judge everyone, will ya.";

				//WriteMessage("Ok, now you got 10 extra souls.",1.7f,0.25f,0f);
				//WriteMessage("Judge everyone, will ya",1.8f,0.25f,0f);
				messageCount++;
				time=0;
			}
		}

		if(messageCount == 12 && GlobalManager.globalManager.population == 0)
		{
			if(time>=messagePass)
			{
				GlobalManager.globalManager.incrementSouls(10);

				tutorialLabel.text = "You have killed them all !! Very good, I think you'll like what's coming next.";

				//WriteMessage("You have killed them all",1.7f,0.25f,0f);
				//WriteMessage("Very good, I think you'll like what's coming next",1.8f,0.25f,0f);
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
			tutorialLabel.text = "Looks like you are going to have to try again. Just press the Redo button up in the left.";

			//WriteMessage("Looks like you are going to have to try again",1.7f,0.25f,0f);
			//WriteMessage("Just press the Redo button up in the left",1.8f,0.25f,0f);
		}
		else
		{
			tutorialLabel.text = "You have killed them all. Very good, I think you'll like what's coming next.";

			//WriteMessage("You have killed them all",1.7f,0.25f,0f);
			//WriteMessage("Very good, I think you'll like what's coming next",1.8f,0.25f,0f);
			messageCount++;
		}
		

	}
}