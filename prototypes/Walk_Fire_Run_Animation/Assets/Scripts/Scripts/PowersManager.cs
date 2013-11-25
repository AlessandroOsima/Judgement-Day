using UnityEngine;
using System.Collections;

public class PowersManager : MonoBehaviour 
{
	//Public Variables
	public Power[] powers;
	//Private Variables
	static PowersManager _powersManager;
	Power selected;
	//Events
	public delegate void onPowerEnabled(Power power);
	public delegate void onPowerDisabled(Power power);
	public event onPowerEnabled powerEnabled;
	public event onPowerDisabled powerDisabled;

	public static PowersManager powersManager
	{
		get
		{
			return _powersManager;
		}
	}
	
	void Awake()
	{
		if(powersManager == null)
			_powersManager = this;
	}
	
	void Start () 
	{
		LevelGUI.levelGUI.setUpPowersBar(powers);
		GlobalManager.globalManager.onSoulsChanged += onSoulsChanged;
	}

	void onSoulsChanged(int pastSouls, int newSouls)
	{
		refreshPowersStates(newSouls);
	}
	//Set the powers state based on the number of souls, powerEnabled and powerDisabled events are called when a power changes state to enabled or disabled
	public void refreshPowersStates(int newSouls)
	{
		foreach(var power in powers)
		{
			if(power.price <= newSouls && power.powerState == PowerState.Disabled && powerEnabled != null)
			{
				power.powerState = PowerState.Enabled;
				powerEnabled(power);
			}

			if(power.powerState == PowerState.ForceDisabled && powerDisabled != null)
			{
				powerDisabled(power);
			}

			if(power.price > newSouls && power.powerState == PowerState.Active && powerDisabled != null)
			{
				power.powerState = PowerState.Disabled;
				powerDisabled(power);
			}

			if (power.price > newSouls && power.powerState == PowerState.Enabled && powerDisabled != null)
			{
				power.powerState = PowerState.Disabled;
				powerDisabled(power);
			}
		}
	}

	//Same as refreshPowersStates but with only one power
	public void refreshPowerState(Power power, int newSouls)
	{
		if(power.price <= newSouls && power.powerState == PowerState.Disabled && powerEnabled != null)
		{
			power.powerState = PowerState.Enabled;
			powerEnabled(power);
		}
		
		if(power.powerState == PowerState.ForceDisabled && powerDisabled != null)
		{
			powerDisabled(power);
		}
		
		if(power.price > newSouls && power.powerState == PowerState.Active && powerDisabled != null)
		{
			power.powerState = PowerState.Disabled;
			powerDisabled(power);
		}
		
		if (power.price > newSouls && power.powerState == PowerState.Enabled && powerDisabled != null)
		{
			power.powerState = PowerState.Disabled;
			powerDisabled(power);
		}
	}
	
	//Used by the GUILevel script, DO NOT call this from other scripts 
	public void onPowerButtonPressed(Power power) 
	{

		if(selected == null)
		{
			power.enabled = true;
			power.powerState = PowerState.Active;
			selected = power;
		} else if(selected != power)
		{
			selected.enabled = false;
			selected.powerState = PowerState.Enabled;
		    selected = power;
			power.powerState = PowerState.Active;
			selected.enabled = true;
		}

	}

	//Used by the GUILevel script, DO NOT use this from other scripts 
	public void onPowerButtonReleased(Power power) 
	{
		selected = null;
		power.enabled = false;
		if(power.powerState == PowerState.Active)
			power.powerState = PowerState.Enabled;
	}
}
