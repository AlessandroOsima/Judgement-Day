﻿using UnityEngine;
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
	public GameObject powerButton;
	public UILabel soulsText;
	public UILabel deathText;
	public UILabel populationText;
	public UILabel soulsVar;
	public UILabel populationVar;
	public UILabel deathsVar;
	public GameObject messageLabel;
	public static Color sGreen = new Color(0.46f,0.86f,0.13f,1);
	public static Color sBlue = new Color(0.12f,0.70f,0.87f,1);
	public static Color sRed = new Color(0.85f,0.24f,0.14f);
	
	//Private Variables
	List<PowerContainer> _powersBar;
	Dictionary<int,int> _powerBarMappings;
	TweenPosition tweenPositionSouls;
	TweenPosition tweenPositionPopulation;
	TweenPosition tweenPositionDeaths;
	TweenAlpha tweenAlphaSouls;
	TweenAlpha tweenAlphaPopulation;
	TweenAlpha tweenAlphaDeaths;
	
	UIRoot root;
	Vector3 scaleFactor = new Vector3(0.4f,0.4f,1f);
	float textScaleFactor = 1.4f;
	int powerJoy;
	
	
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
		this.GetComponent<UIRoot>().manualHeight = Screen.height;
		root = this.GetComponent<UIRoot>();
		
		messages = new List<TextMessage>();
		TextMessage fakeTextMessage = new TextMessage();
		fakeTextMessage.duration = 0;
		messages.Add(fakeTextMessage);
		powerJoy=-1;
		
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

		//Joystick Controls
		if(Input.GetKeyDown(KeyCode.JoystickButton2))
		{
			simulatePowerButtonPress(powerJoy);
			powerJoy++;
			if(powerJoy>=PowersManager.length){
				powerJoy=0;
			}
			simulatePowerButtonPress(powerJoy);
		}
		
		if(Input.GetKeyDown(KeyCode.JoystickButton3))
		{
			simulatePowerButtonPress(powerJoy);
			powerJoy--;
			if(powerJoy<0){
				powerJoy=PowersManager.length-1;
			}
			simulatePowerButtonPress(powerJoy);
		}

		if(Input.GetKeyDown(KeyCode.Joystick1Button7))
			Application.LoadLevel(Application.loadedLevel);

		if(Input.GetKeyDown(KeyCode.Joystick1Button6))
			Application.LoadLevel("LevelSelectionMenu");
		
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
		
		//Debug.Log (" bn " + buttonNumber + ", mbn " + mappedButtonNumber);
		
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
		if(pastPopulation == newPopulation)
		{
			population = newPopulation;
			return;
		}
		
		int populationDifference = newPopulation - pastPopulation;
		
		applyTweenEvents(populationVar,populationDifference,tweenPositionPopulation,tweenAlphaPopulation,populationText,sGreen,sGreen);
		
		population = newPopulation;
	}
	
	//change the souls
	void changeSouls(int pastSouls, int newSouls)
	{
		if(pastSouls == newSouls)
		{
			souls = newSouls;
			return;
		}
		
		int soulsDifference  = newSouls - pastSouls;
		
		applyTweenEvents(soulsVar,soulsDifference,tweenPositionSouls, tweenAlphaSouls, soulsText,sGreen,sRed);
		
		souls = newSouls;
	}
	
	void applyTweenEvents(UILabel label, int difference, TweenPosition positionTween, TweenAlpha alphaTween, UILabel destinationLabel, Color positveColor, Color negativeColor,float duration = 2.5f)
	{
		
		if(difference > 0)
			label.text = "+ " + difference.ToString();
		else
			label.text = difference.ToString();
		
		positionTween = label.GetComponent<TweenPosition>();
		alphaTween = label.GetComponent<TweenAlpha>();
		
		if(positionTween == null)
		{
			positionTween = label.gameObject.AddMissingComponent<TweenPosition>();
			positionTween.from = new Vector3(label.transform.localPosition.x ,label.transform.localPosition.y,0);
			positionTween.to = new Vector3(label.transform.localPosition.x ,(root.activeHeight/2) - (destinationLabel.height/2),0);
			positionTween.duration = duration;
			positionTween.style = UITweener.Style.Once;
			positionTween.Play();
		}
		else
		{
			positionTween.ResetToBeginning();
			positionTween.Play();
		}
		
		//tweenPosition.onFinished.Add(EventDelegate(this.OnTweenFinished));
		
		if(difference > 0)
			label.color = positveColor;
		else
			label.color = negativeColor;
		
		
		if(alphaTween == null)
		{
			alphaTween = label.gameObject.AddMissingComponent<TweenAlpha>();
			alphaTween.from = 1f;
			alphaTween.to = 0f;
			alphaTween.duration = duration;
			alphaTween.style = UITweener.Style.Once;
			alphaTween.Play();
		}
		else
		{
			alphaTween.ResetToBeginning();
			alphaTween.Play();
		}
		
		
	}
	
	//change the deaths
	void changeDeath(int pastDeaths, int newDeaths)
	{
		if(pastDeaths == newDeaths)
		{
			death = newDeaths;
			return;
		}
		
		int deathsDifference = newDeaths - pastDeaths;
		
		applyTweenEvents(deathsVar,deathsDifference,tweenPositionDeaths,tweenAlphaDeaths,deathText,sGreen,sRed);
		
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

			if(power.powerState == PowerState.Disabled || power.powerState == PowerState.ForceDisabled)
			{
				container.powerButton.value = false;
				container.powerButton.enabled = false;
				container.powerSprite.color = new Color(container.powerSprite.color.r,container.powerSprite.color.g,container.powerSprite.color.b,0.5f);
				container.cost.color = new Color(sBlue.r,sBlue.g,sBlue.b,0.5f);
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
		
	}
	
	public void writeWord(string message, Vector3 position, Vector3 scale, float duration, bool useBackground = true)
	{
	}
	
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