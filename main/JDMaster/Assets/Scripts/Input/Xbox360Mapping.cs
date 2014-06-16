using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Xbox360Controller : BaseController
{
	Dictionary<Actions,Axis> axisMapping;
	
	public Xbox360Controller()
	{
		axisMapping = new Dictionary<Actions,Axis> ();
	}

	public override bool useOnPlatform(string controllerName)
	{
        //sets the controller name
        base.useOnPlatform(controllerName);

		if (controllerName.Contains ("Wireless") || controllerName.Contains ("Xbox"))
						return true;

		return false;
	}
	
	public override void Start()
	{
		
		var os = SystemInfo.operatingSystem;

		if (os.Contains ("OS X")) 
		{
			//axisMapping.Add(Actions.Horizontal,new ButtonAxis(KeyCode.JoystickButton8,KeyCode.JoystickButton7));
			//axisMapping.Add(Actions.Vertical,new ButtonAxis(KeyCode.JoystickButton5,KeyCode.JoystickButton6));
			axisMapping.Add(Actions.Horizontal,new UnityAxis("Joy1 Axis 1")); //Main Stick
			axisMapping.Add(Actions.Vertical,new UnityAxis("Joy1 Axis 2",true)); //Main Stick

			axisMapping.Add(Actions.Use,new PositiveButtonAxis(KeyCode.JoystickButton16)); // A button
			axisMapping.Add(Actions.PowerPrev,new PositiveButtonAxis(KeyCode.JoystickButton19)); //X button
			axisMapping.Add(Actions.PowerNext,new  PositiveButtonAxis(KeyCode.JoystickButton18)); //Y button
			axisMapping.Add(Actions.Zoom,new ButtonAxis(KeyCode.JoystickButton13,KeyCode.JoystickButton14)); //D-Pads buttons
			axisMapping.Add(Actions.Quit,new  PositiveButtonAxis(KeyCode.JoystickButton10));
			axisMapping.Add(Actions.Retry,new  PositiveButtonAxis(KeyCode.JoystickButton9));
			axisMapping.Add(Actions.Skip,new  PositiveButtonAxis(KeyCode.JoystickButton17));

		} 
		else if (os.Contains ("Windows")) 
		{
			axisMapping.Add(Actions.Use,new PositiveButtonAxis(KeyCode.JoystickButton0)); // A button
			axisMapping.Add(Actions.Horizontal,new UnityAxis("Joy1 Axis 1"));
			axisMapping.Add(Actions.Vertical,new UnityAxis("Joy1 Axis 2",true));

			axisMapping.Add(Actions.PowerPrev,new PositiveButtonAxis(KeyCode.JoystickButton3));
			axisMapping.Add(Actions.PowerNext,new  PositiveButtonAxis(KeyCode.JoystickButton2));
			axisMapping.Add(Actions.Zoom,new ButtonAxis(KeyCode.JoystickButton4,KeyCode.JoystickButton5));
			axisMapping.Add(Actions.Quit,new  PositiveButtonAxis(KeyCode.JoystickButton6));
			axisMapping.Add(Actions.Retry,new  PositiveButtonAxis(KeyCode.JoystickButton7));
			axisMapping.Add(Actions.Skip,new  PositiveButtonAxis(KeyCode.JoystickButton1));

			//axisMapping.Add(Actions.Horizontal,new ButtonAxis(KeyCode.JoystickButton7,KeyCode.JoystickButton8));
		} 
		else if (os.Contains ("Linux"))
		{
			axisMapping.Add(Actions.Use,new PositiveButtonAxis(KeyCode.JoystickButton0)); // A button
			axisMapping.Add(Actions.Horizontal,new UnityAxis("Joy1 Axis 1"));
			axisMapping.Add(Actions.Vertical,new UnityAxis("Joy1 Axis 2",true));

			axisMapping.Add(Actions.PowerPrev,new PositiveButtonAxis(KeyCode.JoystickButton3));
			axisMapping.Add(Actions.PowerNext,new  PositiveButtonAxis(KeyCode.JoystickButton2));
			axisMapping.Add(Actions.Zoom,new ButtonAxis(KeyCode.JoystickButton4,KeyCode.JoystickButton5));
			axisMapping.Add(Actions.Quit,new  PositiveButtonAxis(KeyCode.JoystickButton10));
			axisMapping.Add(Actions.Retry,new  PositiveButtonAxis(KeyCode.JoystickButton9));
			axisMapping.Add(Actions.Skip,new  PositiveButtonAxis(KeyCode.JoystickButton17));
			axisMapping.Add(Actions.Quit,new  PositiveButtonAxis(KeyCode.JoystickButton6));
			axisMapping.Add(Actions.Retry,new  PositiveButtonAxis(KeyCode.JoystickButton7));
			axisMapping.Add(Actions.Skip,new  PositiveButtonAxis(KeyCode.JoystickButton1));
			
			//axisMapping.Add(Actions.Horizontal,new ButtonAxis(KeyCode.JoystickButton7,KeyCode.JoystickButton8));
		}

		//Xbox controller uses only the PowerPrev and PowerNext actions
		axisMapping.Add(Actions.Power0,new EmptyAxis());
		axisMapping.Add(Actions.Power1,new EmptyAxis());
		axisMapping.Add(Actions.Power2,new EmptyAxis());
		axisMapping.Add(Actions.Power3,new EmptyAxis());
		axisMapping.Add(Actions.Power4,new EmptyAxis());
		axisMapping.Add(Actions.Power5,new EmptyAxis());
		axisMapping.Add(Actions.Power6,new EmptyAxis());
		axisMapping.Add(Actions.Power7,new EmptyAxis());
		axisMapping.Add(Actions.Power8,new EmptyAxis());
		axisMapping.Add(Actions.Power9,new EmptyAxis());

		axisMapping.Add(Actions.RotateEnabled,new EmptyAxis());
		axisMapping.Add(Actions.RotateHorizontal,new EmptyAxis());
		axisMapping.Add(Actions.RotateVertical,new EmptyAxis());
	}
	
	public override string ID()
	{
		return "Xbox360Pad";
	}
}

public class XboxOneController : BaseController
{

    Dictionary<Actions,Axis> axisMapping;

    public XboxOneController()
	{
        axisMapping = new Dictionary<Actions, Axis>();
	}

	public override bool useOnPlatform(string controllerName)
	{
        base.useOnPlatform(controllerName);

        var os = SystemInfo.operatingSystem;

        if (controllerName.Contains("One") && os.Contains("Windows"))
		    return true;

		return false;
	}

    public override string ID()
    {
        return "XboxOnePad";
    }
}