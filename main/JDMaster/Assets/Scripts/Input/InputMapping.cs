using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Actions that happens in the game
//Mapped on floats with ranges 1.0,-1.0 for axis and 1.0(button down), 0.0(button up) for buttons
public enum Actions 
{
	Horizontal, //horizontal movement, around x axis [1.0,-1.0]
	Vertical, //vertical movement, around y axis [1.0,-1.0]
	RotateEnabled, //enables rotation [1.0,0.0]
	RotateHorizontal, //rotation [1.0,-1.0]
	RotateVertical, //rotation [1.0,-1.0]
	PowerPrev,
	PowerNext,
	Power0, //All the button to select the powers in the power bar, from [1.0,0.0] 
	Power1,
	Power2,
	Power3,
	Power4,
	Power5,
	Power6,
	Power7,
	Power8,
	Power9,
	Quit,
	Retry,
	Skip,
	Zoom, //zoom the camera [1.0,-1.0]
	Use //use a power or select an item/level [1.0,0.0]
};

//An action controller is a device from wich the game reads Inputs (like a keyboard or a joypad). Only one ActionController can be used at a time
public abstract class ActionController
{
	public abstract void Start(); //init the controller when it's recognized
	public abstract float GetAction(Actions action); //return an action val
	public abstract string ID(); //human readable string representing a controller
	public abstract bool useOnPlatform(string controllerName);
    public abstract Vector3 GetActionPosition(Actions action);
}

//An axis can move from a range of [1.0,-1.0] just like in the Unity native Input Manager
public abstract class Axis
{
	public abstract float getAxis ();
}

//Always returns 0, use it when you don't need an action to work on a particular controller
public class EmptyAxis : Axis
{
	public override float getAxis ()
	{
		return 0;
	}
}

//Simulate an axis by using postive (returns 1.0) and negative buttons (returns -1.0).
public class ButtonAxis : Axis 
{
	KeyCode positiveButton;
	KeyCode negativeButton;

	KeyCode altPositiveButton;
	KeyCode altNegativeButton;

	bool isAltUsed = false;

	public ButtonAxis(KeyCode PositiveButton, KeyCode NegativeButton)
	{
		positiveButton = PositiveButton;
		negativeButton = NegativeButton;
	}

	public ButtonAxis(KeyCode PositiveButton, KeyCode NegativeButton, KeyCode AltPositiveButton, KeyCode AltNegativeButton)
	{
		positiveButton = PositiveButton;
		altPositiveButton = AltPositiveButton;
		
		negativeButton = NegativeButton;
		altNegativeButton = AltNegativeButton;

		isAltUsed = true;
	}

	public override float getAxis () // if both postive and negative buttons are pressed the movement is 0.0f
	{
		float val = 0;

		if (isAltUsed) 
		{
			if (Input.GetKey (positiveButton) || Input.GetKey (altPositiveButton))
								val = 1.0f;

			if (Input.GetKey (negativeButton) || Input.GetKey (altNegativeButton))
								val -= 1.0f;
		}
		else
		{
			if (Input.GetKey (positiveButton))
				val = 1.0f;
			
			if (Input.GetKey (negativeButton))
				val -= 1.0f;
		}

		return val;
	}
}

//Simulate a button by using postive (returns 1.0 if button down) buttons. Basically a button.
public class PositiveButtonAxis : Axis 
{
	KeyCode positiveButton;
	KeyCode altPositiveButton;
	bool isAltUsed = false;

	public PositiveButtonAxis(KeyCode PositiveButton)
	{
		positiveButton = PositiveButton;
	}

	public PositiveButtonAxis(KeyCode PositiveButton, KeyCode AltPositiveButton)
	{
		positiveButton = PositiveButton;
		altPositiveButton = AltPositiveButton;
		isAltUsed = true;
	}
	
	public override float getAxis () // if both postive and negative buttons are pressed the movement is 0.0f
	{
		if(isAltUsed)
		{
			if (Input.GetKeyDown (positiveButton) || Input.GetKeyDown (altPositiveButton))
				return 1.0f;
		}
		else
		{
			if (Input.GetKeyDown(positiveButton))
				return 1.0f;
		}

		return 0f;
	}
}

//Calls and axis from the Unity Input manager
public class UnityAxis : Axis
{
	private string axisName;
	private bool invert;

	public bool invertAxis
	{
		get
		{
			return invert;
		}

		set
		{
			invert = value;
		}
	}

	public UnityAxis(string AxisName, bool invertAxis = false)
	{
		axisName = AxisName;
		invert = invertAxis;
	}

	public override float getAxis ()
	{
		if(invert)
		return -Input.GetAxis (axisName) ;
		else
		return Input.GetAxis (axisName);
	}
}


public class UnitySplitAxis : Axis
{
	public enum Side
	{
		Positive,
		Negative
	};

	private Side side;
	private string axisName;
	
	public UnitySplitAxis(string AxisName, Side axisSide)
	{
		axisName = AxisName;
		side = axisSide;
	}
	
	public override float getAxis ()
	{
		if(side == Side.Positive)
		{
			if(Input.GetAxis(axisName) >= 0)
			{
				return  Input.GetAxis (axisName);
			}
			return 0;
		}
		else
		{
			if(Input.GetAxis (axisName) <= 0 )
			{
				return  -Input.GetAxis (axisName);
			}

			return 0;
		}
	}
}

public static class InputMapping 
{
	private static List<ActionController> controllers;
	private static bool isOnPC = false; //if on OsX, Windows or Mac
	private static int currentController = 1;

	static InputMapping()
	{
		RegisterControllers ();
	}

	public static List<string> getValidControllersID()
	{
		if (controllers.Count == 0)
			 return null;

		List<string> ids = new List<string>();

		for(int i = 0; i < controllers.Count; i++) 
		{
			ids.Add(controllers[i].ID());
		}

		return ids;
	}

	public static int getValidControllersCount()
	{
		return controllers.Count;
	}
	
	public static void useController(int controllerNumber) 
	{
		if (controllerNumber >= 0 && controllerNumber < controllers.Count)
			currentController = controllerNumber;
	}

	public static int getCurrentController() 
	{
		return currentController;
	}

	public static void RegisterControllers()
	{
		controllers = new List<ActionController> ();
		//initialize correct controller
		var os = SystemInfo.operatingSystem;

		Debug.Log (os);
		var keyboard = new KeyboardController ();

		if (keyboard.useOnPlatform("keyboard")) 
		{
				controllers.Add (keyboard);
				isOnPC = true;
		}
		
#if !UNITY_WP8 
		var joysticks = Input.GetJoystickNames();

		Debug.Log("Found " + joysticks.Length + " joysticks");

		//instantiate valid controller for every platfom
		foreach (var joystick in joysticks) 
		{
            var Xbox360Pad = new Xbox360Controller();

			if(Xbox360Pad.useOnPlatform(joystick))
			{
				Xbox360Pad.Start();
				controllers.Add (Xbox360Pad);

			}
		}
#endif
        if (controllers.Count == 1 && isOnPC)
            currentController = 0; //Use keyboard if no controller and on PC
        else if (isOnPC)
            currentController = 1;

#if UNITY_WP8
		var touchSensor = new TouchController();
		if (touchSensor.useOnPlatform("touch"))
		{
				controllers.Add (touchSensor);
		}
#endif

	}



	public static float GetAction(Actions action)
	{
		if (controllers.Count != 0 && currentController >= 0 && currentController < controllers.Count) 
		{
			return controllers[currentController].GetAction(action);
		}

		#if DEBUG
		Debug.LogWarning("No valid Controller Found");
		#endif

		//RegisterControllers ();

		return 0f;
	}

    public static Vector3 GetActionPosition(Actions action)
    {
        if (controllers.Count != 0 && currentController >= 0 && currentController < controllers.Count)
        {
            return controllers[currentController].GetActionPosition(action);
        }

#if DEBUG
		Debug.LogWarning("No valid Controller Found");
#endif

        //RegisterControllers ();

        return Vector3.zero;
    }
}

