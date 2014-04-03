using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGUI : MonoBehaviour 
{
	struct PowerContainer
	{
		public UISprite powerSprite;
		public UIToggle powerButton;
		public UILabel cost;
		public Power power;
	}

	class TextMessage
	{
		public UILabel text;
		public UISprite background;
		public float timer;
		public float duration;
	}

	//Public Variables
	static LevelGUI _levelGUI;
	//public UIToolkit GUIToolkit;
	//public UIToolkit PowersGUIToolkit;
	public GameObject powerButton;
	public UILabel soulsText;
	public UILabel deathText;
	public UILabel populationText;
	public GameObject messageLabel;
	public static Color sGreen = new Color(0.46f,0.86f,0.13f,1);
	public static Color sBlue = new Color(0.12f,0.70f,0.87f,1);
	public static Color sRed = new Color(0.85f,0.24f,0.14f);

	//Private Variables
	List<PowerContainer> _powersBar;
	Dictionary<int,int> _powerBarMappings;

	UIRoot root;
	Vector3 scaleFactor = new Vector3(0.4f,0.4f,1f);
	float textScaleFactor = 1.4f;


	List<TextMessage> messages;
	UIToggle selectedPower = null;
	
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
			soulsText.text  = value.ToString();
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
		this.GetComponent<UIRoot> ().manualHeight = Screen.height;
		root = this.GetComponent<UIRoot>();

		messages = new List<TextMessage>();
		TextMessage fakeTextMessage = new TextMessage();
		fakeTextMessage.duration = 0;
		messages.Add(fakeTextMessage);

		_levelGUI = this;

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
						messages[i].text.text = "";
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

		var mappedButtonNumber = _powerBarMappings[buttonNumber];

		Debug.Log (" bn " + buttonNumber + ", mbn " + mappedButtonNumber);

		if(!_powersBar[mappedButtonNumber].powerButton.enabled)
			return;

		if(_powersBar[mappedButtonNumber].powerButton.value == true)
			_powersBar[mappedButtonNumber].powerButton.value = false;
		else
			_powersBar[mappedButtonNumber].powerButton.value = true;
		
		onPowerButtonPressed(_powersBar[mappedButtonNumber].powerButton);
	}

	//PowersManager events
	//Enable a Power in the powers bar
	void changePowersEnabled(Power power)
	{
		foreach(var powerContainer in _powersBar)
		{
			if(powerContainer.power == power)
			{
				powerContainer.powerButton.enabled = true;
				powerContainer.powerSprite.color = new Color(powerContainer.powerSprite.color.r,powerContainer.powerSprite.color.g,powerContainer.powerSprite.color.b,1f);
				powerContainer.cost.color = new Color(sBlue.r,sBlue.g,sBlue.b,1f);
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

				powerContainer.powerButton.value = false;
				powerContainer.powerButton.enabled = false;
				powerContainer.powerSprite.color = new Color(powerContainer.powerSprite.color.r,powerContainer.powerSprite.color.g,powerContainer.powerSprite.color.b,0.5f);
				powerContainer.cost.color = new Color(sBlue.r,sBlue.g,sBlue.b,0.5f);
				
				if(selectedPower != null && power == ((PowerContainer)selectedPower.UserData).power)
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
				messages[i].text.text = "";
				Destroy(messages[i].text);
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
		_powerBarMappings = new Dictionary<int, int>();

		var xPos = (-Screen.width/2  + powerButton.GetComponent<UISprite>().width/2) * root.pixelSizeAdjustment;
		int  halfSpacer = powers.Length/2;
		var startYPos = 0;
		int posCounter = 1;



		foreach(var power in powers)
		{
			GameObject newButton = NGUITools.AddChild(this.gameObject,powerButton);

			UIToggle nbScript = newButton.GetComponent<UIToggle>();
			UILabel nbLabel = newButton.GetComponentInChildren<UILabel>();
			nbLabel.text = ((Power)power).price.ToString();

			newButton.transform.localPosition = new Vector3(xPos,startYPos * root.pixelSizeAdjustment,0);
			newButton.GetComponent<UISprite>().spriteName = power.name;
			newButton.transform.FindChild("Selected").GetComponent<UISprite>().spriteName = power.name + "_Over";

			PowerContainer container = new PowerContainer();
			container.powerSprite = newButton.GetComponent<UISprite>();
			container.powerButton = nbScript;
			container.cost = nbLabel;
			container.power = power;
			nbScript.UserData = container;
			_powersBar.Add(container);

			if(posCounter <= halfSpacer)
				_powerBarMappings.Add(posCounter - 1,halfSpacer - posCounter);

			posCounter++; 

			if(posCounter == halfSpacer + 1)
				startYPos = 0;

			if(posCounter > halfSpacer)
			{
				startYPos -= powerButton.GetComponent<UISprite> ().height;
				_powerBarMappings.Add(posCounter - 1,posCounter - 1);
			}
			else
			{
				startYPos += powerButton.GetComponent<UISprite> ().height;
			}
		}
	}

	public void WriteMessage(string s, float h,float w, float d, Color textColor,bool outline = false, int fontSize = 50)
	{
		if(messages == null)
		{
			messages = new List<TextMessage>();
			TextMessage fakeTextMessage = new TextMessage();
			fakeTextMessage.duration = 0;
			messages.Add(fakeTextMessage);
		}

		TextMessage textMessage = new TextMessage();
		GameObject message = NGUITools.AddChild (this.gameObject, messageLabel);

		textMessage.text = message.GetComponent<UILabel>();
		textMessage.text.text = s;

		if(outline)
			textMessage.text.effectStyle = UILabel.Effect.Outline;

	    if (fontSize > 0)
			textMessage.text.fontSize = fontSize;

		textMessage.text.color = textColor;
		textMessage.duration = 0;
		textMessage.timer = 0;

		messages.Add(textMessage);
		
		/*
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
		*/
	}

	public void writeWord(string message, Vector3 position, Vector3 scale, float duration, bool useBackground = true)
	{
	}

	/*
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

		/*
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
   */

	public bool isDisplayingMessages()
	{
	
		return messages.Count > 1;
	}

	//Callback for onTouchUpInside on every button in the power bar, used to notify the PowersManager for powers activation/deactivation

	public void onPowerButtonPressed(UIToggle sender)
	{
		if (sender.value) 
		{
			if(selectedPower != sender)
			{
				if(selectedPower != null)
				{
					((PowerContainer)selectedPower.UserData).cost.color = sBlue;
					selectedPower.value = false;
				}
				
				selectedPower = sender;
				
				((PowerContainer)selectedPower.UserData).cost.color = sRed;
				PowersManager.powersManager.onPowerButtonPressed(((PowerContainer) sender.UserData).power);
			}
		} 
		else 
		{
			if(selectedPower != null)
			{
				((PowerContainer)selectedPower.UserData).cost.color = sBlue;
				selectedPower = null;
				PowersManager.powersManager.onPowerButtonReleased(((PowerContainer) sender.UserData).power);
			}
		}
	}


	public void OnQuitButtonPress()
	{
	   Application.LoadLevel ("LevelSelectionMenu");
	}

	public void OnReplayButtonPress()
	{
		Application.LoadLevel (Application.loadedLevelName);
	}
	
 }