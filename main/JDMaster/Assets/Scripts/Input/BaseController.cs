using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*  Base Controller, it's instantiated with every kind of controller and defines basic mappings for every controller
 *  The mappings are the same ones as the Xbox 360 controller on windows
 */
public class BaseController : ActionController
{
    Dictionary<Actions, Axis> axisMapping;
    string friendlyID = "base controller";

    public BaseController()
    {
        axisMapping = new Dictionary<Actions, Axis>();
    }

    public override bool useOnPlatform(string controllerName)
    {
            friendlyID = controllerName;
            return true;
    }

    public override void Start()
    {

        var os = SystemInfo.operatingSystem;

        axisMapping.Add(Actions.Use, new PositiveButtonAxis(KeyCode.JoystickButton0)); // A button
        axisMapping.Add(Actions.Horizontal, new UnityAxis("Joy1 Axis 1"));
        axisMapping.Add(Actions.Vertical, new UnityAxis("Joy1 Axis 2", true));

        axisMapping.Add(Actions.PowerPrev, new PositiveButtonAxis(KeyCode.JoystickButton3));
        axisMapping.Add(Actions.PowerNext, new PositiveButtonAxis(KeyCode.JoystickButton2));
        axisMapping.Add(Actions.Zoom, new ButtonAxis(KeyCode.JoystickButton4, KeyCode.JoystickButton5));
        axisMapping.Add(Actions.Quit, new PositiveButtonAxis(KeyCode.JoystickButton6));
        axisMapping.Add(Actions.Retry, new PositiveButtonAxis(KeyCode.JoystickButton7));
        axisMapping.Add(Actions.Skip, new PositiveButtonAxis(KeyCode.JoystickButton1));

        axisMapping.Add(Actions.Power0, new EmptyAxis());
        axisMapping.Add(Actions.Power1, new EmptyAxis());
        axisMapping.Add(Actions.Power2, new EmptyAxis());
        axisMapping.Add(Actions.Power3, new EmptyAxis());
        axisMapping.Add(Actions.Power4, new EmptyAxis());
        axisMapping.Add(Actions.Power5, new EmptyAxis());
        axisMapping.Add(Actions.Power6, new EmptyAxis());
        axisMapping.Add(Actions.Power7, new EmptyAxis());
        axisMapping.Add(Actions.Power8, new EmptyAxis());
        axisMapping.Add(Actions.Power9, new EmptyAxis());

        axisMapping.Add(Actions.RotateEnabled, new EmptyAxis());
        axisMapping.Add(Actions.RotateHorizontal, new EmptyAxis());
        axisMapping.Add(Actions.RotateVertical, new EmptyAxis());
    }

    public override float GetAction(Actions action)
    {
        Axis axis;
        axisMapping.TryGetValue(action, out axis);
        return axis.getAxis();
    }

    public override Vector3 GetActionPosition(Actions action)
    {
        return Vector3.zero;
    }

    public override string FriendlyID()
    {
        return friendlyID;
    }

    public override string ID()
    {
        return "baseController";
    }
}