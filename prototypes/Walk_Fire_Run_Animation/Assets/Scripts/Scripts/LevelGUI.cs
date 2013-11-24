using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGUI : MonoBehaviour 
{
	
	//Public Variables
	static LevelGUI _levelGUI;
	public UIToolkit GUIToolkit;
	public UIToolkit PowersGUIToolkit;
	//Private Variables
	List<UIButton> _powersBar;
	Color sGreen = new Color(0.46f,0.86f,0.13f,1);
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
		levelGUI = this;
		//UI
		quitButton =  UIButton.create(GUIToolkit,"Close.png","Close_Over.png",0,0);
		quitButton.onTouchUpInside += sender => Application.Quit();
		quitButton.scale = scaleFactor;

		replayButton = UIButton.create(GUIToolkit,"Replay.png","Replay_Over.png",0,0);
		replayButton.pixelsFromTopLeft(0,(int)quitButton.width);
		replayButton.onTouchUpInside += sender => Application.LoadLevel("Level 01_Island");
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

	//PowersManager events
	//Enable a Power in the powers bar
	void changePowersEnabled(Power power)
	{
		foreach(var powerButton in _powersBar)
		{
			if(powerButton.userData == power)
		    {
				powerButton.disabled = false;
				powerButton.color = new Color(powerButton.color.r,powerButton.color.g,powerButton.color.b,1f);
			}
		}

	}

	//Disable a Power in the powers bar
	void changePowersDisabled(Power power)
	{
		foreach(var powerButton in _powersBar)
		{
			if(powerButton.userData == power)
			{
				powerButton.disabled = true;
				powerButton.color = new Color(powerButton.color.r,powerButton.color.g,powerButton.color.b,0.5f);

				if(selectedPower != null && power == (Power)selectedPower.userData)
				{
					selectedPower = null;
					PowersManager.powersManager.onPowerButtonReleased(power);
				}
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

		_powersBar = new List<UIButton>();


		int spacer = (int)UIRelative.yPercentFrom(UIyAnchor.Top, Screen.height, 1f/(float)powers.Length);
		int lastButtonHeight = + spacer;

		foreach(var power in powers)
		{
			UIButton powerButton = UIButton.create(PowersGUIToolkit,power.name + ".png",power.name + "_Over.png",0,0);
			powerButton.keepOn = true;
			powerButton.scale = scaleFactor * 1.5f;
			powerButton.pixelsFromTopLeft(lastButtonHeight,0);
			powerButton.onTouchUpInside += onPowerButtonPressed;
			powerButton.userData = power;
			powerButton.color = new Color(powerButton.color.r,powerButton.color.g,powerButton.color.b,0.5f);
			powerButton.disabled = true;
			_powersBar.Add(powerButton);
			lastButtonHeight += (int)powerButton.height;
		}

	}
	//Callback for onTouchUpInside on every button in the power bar, used to notify the PowersManager for powers activation/deactivation
	void onPowerButtonPressed(UIButton sender)
	{
		if(sender.highlighted) //The power is being activated
		{
			if(selectedPower != sender)
			{
				if(selectedPower != null)
					selectedPower.highlighted = false;

				selectedPower = sender;

				PowersManager.powersManager.onPowerButtonPressed((Power)sender.userData);
			}
		}
		else //The power is being deactivated
		{
			selectedPower = null;
			PowersManager.powersManager.onPowerButtonReleased((Power)sender.userData);
		}
	}
}
