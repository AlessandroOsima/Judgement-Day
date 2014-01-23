using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGUI : MonoBehaviour 
{

	struct PowerContainer
	{
		public UIButton powerButton;
		public UITextInstance textInstance;
		public Power power;
	}

	class TextMessage
	{
		public UITextInstance text;
		public UISprite background;
		public float timer;
		public float duration;
	}

	//Public Variables
	static LevelGUI _levelGUI;
	public UIToolkit GUIToolkit;
	public UIToolkit PowersGUIToolkit;
	public string thisScene;
	//Private Variables
	List<PowerContainer> _powersBar;
	Color sGreen = new Color(0.46f,0.86f,0.13f,1);
	Color sBlue = new Color(0.12f,0.70f,0.87f,1);
	Color sRed = new Color(0.85f,0.24f,0.14f);
	Vector3 scaleFactor = new Vector3(0.4f,0.4f,1f);
	float textScaleFactor = 1.4f;
	UIButton quitButton;
	UIButton replayButton;
	UISprite soulsNumber;
	UISprite deathNumber;
	UISprite populationNumber;
	UIText text;
	UITextInstance soulsText;
	UITextInstance deathText;
	UITextInstance populationText;
	List<TextMessage> messages;
	UIButton selectedPower;
	
	public static LevelGUI levelGUI
	{
		get
		{
			return _levelGUI;
		}
	}
	
	public int souls
	{
		set
		{
			soulsText.text = value.ToString();
		}
	}
	
	public int population
	{
		set
		{
			populationText.text = value.ToString();
		}
	}
	
	public int death
	{
		set
		{
			deathText.text = value.ToString();
		}
	}
	
	
	void Awake()
	{
		//Awake is used by UIToolkit for initialization, DO NOT USE THIS.

	}
	
	// Use this for initialization
	void Start () 
	{       

		messages = new List<TextMessage>();
		TextMessage fakeTextMessage = new TextMessage();
		fakeTextMessage.duration = 0;
		messages.Add(fakeTextMessage);

		_levelGUI = this;

		//UI
		quitButton =  UIButton.create(GUIToolkit,"Close.png","Close_Over.png",0,0);
		quitButton.onTouchUpInside += sender => Application.LoadLevel("SplashScreen");
		quitButton.scale = scaleFactor;
		
		replayButton = UIButton.create(GUIToolkit,"Replay.png","Replay_Over.png",0,0);
		replayButton.pixelsFromTopLeft(0,(int)quitButton.width);
		replayButton.onTouchUpInside += sender => Application.LoadLevel(Application.loadedLevelName);
		replayButton.centerize();
		replayButton.scale = scaleFactor;
		
		soulsNumber = GUIToolkit.addSprite("Souls.png",0,0);
		soulsNumber.positionFromTopRight(0,0);
		soulsNumber.centerize();
		soulsNumber.scale = scaleFactor;
		
		deathNumber = GUIToolkit.addSprite("Deaths.png",0,0);
		deathNumber.pixelsFromTopRight(0,(int)soulsNumber.width);
		deathNumber.centerize();
		deathNumber.scale = scaleFactor;
		
		populationNumber = GUIToolkit.addSprite("Population.png",0,0);
		populationNumber.pixelsFromTopRight(0,(int)(soulsNumber.width + deathNumber.width));
		populationNumber.centerize();
		populationNumber.scale = scaleFactor;
		
		//Load the fonts and initialize the text instances
		text = new UIText(GUIToolkit, "Smilage", "Smilage_0.png" );
		text.forceLowAscii = true;
		
		soulsText = text.addTextInstance(System.Convert.ToString(0),0,0);
		soulsText.textScale =  textScaleFactor;
		soulsText.pixelsFromTopRight((int)(soulsNumber.height / 6),(int)(soulsNumber.width / 1.7f));
		soulsText.setColorForAllLetters(sGreen);
		
		deathText = text.addTextInstance(System.Convert.ToString(0),0,0);
		deathText.textScale = textScaleFactor;
		deathText.pixelsFromTopRight((int)(deathNumber.height / 6),(int)(deathNumber.width / 1.7f + soulsNumber.width));
		deathText.setColorForAllLetters(sGreen);
		
		populationText = text.addTextInstance(System.Convert.ToString(0),0,0);
		populationText.textScale = textScaleFactor;
		populationText.pixelsFromTopRight((int)(populationNumber.height / 6),(int)(populationNumber.width / 1.7f + populationNumber.width + soulsNumber.width));
		populationText.setColorForAllLetters(sGreen);
		
		//Register for global vars and powers events
		GlobalManager.globalManager.onPopulationChanged += changePopulation;
		GlobalManager.globalManager.onScoreChanged += changeDeath;
		GlobalManager.globalManager.onSoulsChanged += changeSouls;
		
		PowersManager.powersManager.powerEnabled += changePowersEnabled;
		PowersManager.powersManager.powerDisabled += changePowersDisabled;
	}


	void Update()
	{
		if(messages != null)
		{
			for(int i = 0; i < messages.Count; i++)
			{
				if(messages[i].duration > 0)
				{
					messages[i].timer += Time.deltaTime;
					if(messages[i].timer >= messages[i].duration)
					{
						messages[i].text.clear();
						messages.Remove(messages[i]);
					}

				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			simulatePowerButtonPress(0);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			simulatePowerButtonPress(1);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			simulatePowerButtonPress(2);
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			simulatePowerButtonPress(3);
		}
		if(Input.GetKeyDown(KeyCode.Alpha5))
		{
			simulatePowerButtonPress(4);
		}
		if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			simulatePowerButtonPress(5);
		}
		if(Input.GetKeyDown(KeyCode.Alpha7))
		{
			simulatePowerButtonPress(6);
		}
		if(Input.GetKeyDown(KeyCode.Alpha8))
		{
			simulatePowerButtonPress(7);
		}
		if(Input.GetKeyDown(KeyCode.Alpha9))
		{
			simulatePowerButtonPress(8);
		}
	}

	void simulatePowerButtonPress(int buttonNumber)
	{
		if(buttonNumber < 0 || buttonNumber > (_powersBar.Count - 1))
			return; 

		if(_powersBar[buttonNumber].powerButton.disabled)
			return;

		if(_powersBar[buttonNumber].powerButton.highlighted == true)
			_powersBar[buttonNumber].powerButton.highlighted = false;
		else
			_powersBar[buttonNumber].powerButton.highlighted = true;
		
		onPowerButtonPressed(_powersBar[buttonNumber].powerButton);
	}

	//PowersManager events
	//Enable a Power in the powers bar
	void changePowersEnabled(Power power)
	{
		foreach(var powerContainer in _powersBar)
		{
			if(powerContainer.power == power)
			{
				powerContainer.powerButton.disabled = false;
				powerContainer.powerButton.color = new Color(powerContainer.powerButton.color.r,powerContainer.powerButton.color.g,powerContainer.powerButton.color.b,1f);
				powerContainer.textInstance.color = new Color(sBlue.r,sBlue.g,sBlue.b,1f);
			}
		}
		
	}
	
	//Disable a Power in the powers bar
	void changePowersDisabled(Power power)
	{
		foreach(var powerContainer in _powersBar)
		{
			if(powerContainer.power == power)
			{
				powerContainer.powerButton.disabled = true;
				powerContainer.powerButton.color = new Color(powerContainer.powerButton.color.r,powerContainer.powerButton.color.g,powerContainer.powerButton.color.b,0.5f);
				powerContainer.textInstance.color = new Color(sBlue.r,sBlue.g,sBlue.b,0.5f);
				
				if(selectedPower != null && power == ((PowerContainer)selectedPower.userData).power)
				{
					selectedPower = null;
					PowersManager.powersManager.onPowerButtonReleased(power);
				}
			}
		}
	}

	public void clearMessages()
	{
		if(messages.Count >= 1)
		{
			int messagesSize = messages.Count;
			for(int i = messagesSize - 1 ; i > 0; i--)
			{
				messages[i].text.clear();
				messages[i].text.destroy();
				messages.Remove(messages[i]);
			}
		}
	}
	
	//GlobalManager events
	//change the population
	void changePopulation(int pastPopulation, int newPopulation)
	{
		population = newPopulation;
	}
	//change the souls
	void changeSouls(int pastSouls, int newSouls)
	{
		souls = newSouls;
	}
	//change the deaths
	void changeDeath(int pastDeaths, int newDeaths)
	{
		death = newDeaths;
	}
	
	//Setup powers bar, called ONLY by power manager with a list of the powers in a level
	public void setUpPowersBar(Power[] powers)
	{
		if(_powersBar != null && powers.Length > 0) // the powers bar is already set up or there are no powers avaliable
			return;
		
		_powersBar = new List<PowerContainer>();
		
		
		int spacer = (int)UIRelative.yPercentFrom(UIyAnchor.Top, Screen.height, 1f/(float)powers.Length);
		int lastButtonHeight = + spacer;
		
		foreach(var power in powers)
		{
			UIButton powerButton = UIButton.create(PowersGUIToolkit,power.name + ".png",power.name + "_Over.png",0,0);
			powerButton.keepOn = true;
			powerButton.scale = scaleFactor * 1.5f;
			powerButton.pixelsFromTopLeft(lastButtonHeight,0);
			powerButton.onTouchUpInside += onPowerButtonPressed;
			powerButton.color = new Color(powerButton.color.r,powerButton.color.g,powerButton.color.b,0.5f);
			powerButton.disabled = true;
			PowerContainer container = new PowerContainer();
			container.powerButton = powerButton;
			container.textInstance = text.addTextInstance(System.Convert.ToString(power.price),0,0);
			container.textInstance.position = new Vector3(powerButton.position.x + (powerButton.width - container.textInstance.width/2),powerButton.position.y - (powerButton.height - container.textInstance.height/2),powerButton.position.z);
			container.textInstance.setColorForAllLetters(sBlue);
			container.textInstance.color = new Color(sBlue.r,sBlue.g,sBlue.b,0.5f);
			container.power = power;
			powerButton.userData = container;
			_powersBar.Add(container);
			lastButtonHeight += (int)powerButton.height;
		}
		
	}

	public void WriteMessage(string s, float h,float w, float d)
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
			space += (word.Length + 2) * 14;
			j++;
			i = j;
		}
		
		word = s.Substring(i);
		LevelGUI.levelGUI.writeWord(word,new Vector3(space, (h * height), 0f), new Vector3(1.2f,1.2f,1),d,true);
	}

	//duration is the time in seconds needed a message will stay visible before being destroyed, if duration <= 0 the message will stay visible forever
	public void writeWord(string message, Vector3 position, Vector3 scale, float duration, bool useBackground = true)
	{
		if(messages == null)
		{
			messages = new List<TextMessage>();
			TextMessage fakeTextMessage = new TextMessage();
			fakeTextMessage.duration = 0;
			messages.Add(fakeTextMessage);
		}

		TextMessage textMessage = new TextMessage();

		textMessage.text = text.addTextInstance(message,position.x,position.y);
		textMessage.text.alignMode = UITextAlignMode.Center;
		textMessage.text.xPos = position.x;
		textMessage.text.yPos = position.y;
		textMessage.text.scale = scale;
		textMessage.text.setColorForAllLetters(sBlue);

		textMessage.duration = duration;
		textMessage.timer = 0;

		if(useBackground)
		{
			//null
		}

		messages.Add(textMessage);
	}

	public bool isDisplayingMessages()
	{
	
		return messages.Count > 1;
	}

	//Callback for onTouchUpInside on every button in the power bar, used to notify the PowersManager for powers activation/deactivation
	void onPowerButtonPressed(UIButton sender)
	{
		if(sender.highlighted) //The power is being activated
		{
			if(selectedPower != sender)
			{
				if(selectedPower != null)
				{
					((PowerContainer)selectedPower.userData).textInstance.setColorForAllLetters(sBlue);
					selectedPower.highlighted = false;
				}
				
				selectedPower = sender;

				((PowerContainer)selectedPower.userData).textInstance.setColorForAllLetters(sRed);
				PowersManager.powersManager.onPowerButtonPressed(((PowerContainer) sender.userData).power);
			}
		}
		else //The power is being deactivated
		{
			if(selectedPower != null)
				((PowerContainer)selectedPower.userData).textInstance.setColorForAllLetters(sBlue);

			selectedPower = null;
			PowersManager.powersManager.onPowerButtonReleased(((PowerContainer) sender.userData).power);
		}
	}
}