using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyboardController : ActionController
{
    private Vector3 clickPosition;
	public override void Start()
	{
		
	}

	public override bool useOnPlatform(string controllerName)
	{

		var os = SystemInfo.operatingSystem;
		
		Debug.Log (os);
		
		if (os.Contains ("OS X") || os.Contains ("Windows") || os.Contains ("Linux")) 
			return true;


		return false;
	}
	
	public override float GetAction(Actions action)
	{
		
		switch(action)
		{
		case Actions.Horizontal :
		{
			return Input.GetAxis("Horizontal");
		}
			
		case Actions.Vertical :
		{
			return Input.GetAxis("Vertical");
		}
			
		case Actions.Zoom :
		{
			return Input.GetAxis("Zoom");
		}
			
		case Actions.Use :
		{
			if (Input.GetKeyDown (KeyCode.Space))
            {
                clickPosition=Vector3.zero;
                return 1.0f;
            }
            if (Input.GetMouseButton(0))
            {
                clickPosition=Input.mousePosition;
                return 1.0f;
            }
			
			return 0;
		}

		case Actions.Quit :
		{
			if(Input.GetKeyDown(KeyCode.Escape))
				return 1.0f;
			
			return 0;
		}

		case Actions.Retry :
		{
			if(Input.GetKeyDown(KeyCode.R))
				return 1.0f;
			
			return 0;
		}

		case Actions.Skip :
		{
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButton(0))
				return 1.0f;
			
			return 0;
		}

		case Actions.RotateEnabled :
		{
			return Input.GetAxis("RotateEnabled");
		} 
			
		case Actions.RotateHorizontal :
		{
			return Input.GetAxis("Mouse Y");
		}
			
		case Actions.RotateVertical :
		{
			return Input.GetAxis("Mouse X");
		}
			
		case Actions.Power0 :
		{
			if(Input.GetKeyDown(KeyCode.Alpha1))
				return 1.0f;
			
			return 0;
		}
			
		case Actions.Power1 :
		{
			if(Input.GetKeyDown(KeyCode.Alpha2))
				return 1.0f;
			
			return 0;
		}
			
		case Actions.Power2 :
		{
			if(Input.GetKeyDown(KeyCode.Alpha3))
				return 1.0f;
			
			return 0;
		}
			
		case Actions.Power3 :
		{
			if(Input.GetKeyDown(KeyCode.Alpha4))
				return 1.0f;
			
			return 0;
		}
			
		case Actions.Power4 :
		{
			if(Input.GetKeyDown(KeyCode.Alpha5))
				return 1.0f;
			
			return 0;
		}
			
		case Actions.Power5 :
		{
			if(Input.GetKeyDown(KeyCode.Alpha6))
				return 1.0f;
			
			return 0;
		}
			
		case Actions.Power6 :
		{
			if(Input.GetKeyDown(KeyCode.Alpha7))
				return 1.0f;
			
			return 0;
		}
			
		case Actions.Power7 :
		{
			if(Input.GetKeyDown(KeyCode.Alpha8))
				return 1.0f;
			
			return 0;
		}
			
		case Actions.Power8 :
		{
			if(Input.GetKeyDown(KeyCode.Alpha9))
				return 1.0f;
			
			return 0;
		}
			
		case Actions.Power9 :
		{
			if(Input.GetKeyDown(KeyCode.Alpha0))
				return 1.0f;
			
			return 0;
		}

		case Actions.PowerNext :
		{
			
			return 0;
		}

		case Actions.PowerPrev :
		{
			
			return 0;
		}

		
			
		default :
		{
			#if DEBUG
			Debug.LogWarning("No key registered in keyboard controller for Action : " + action);
			#endif
			
			return 0.0f;
		}
		}
	}

    public override Vector3 GetActionPosition(Actions action)
    {
        if(action==Actions.Use)
        {
            return clickPosition;
        }
        return Vector3.zero;
    }
	
	public override string ID()
	{
		return "Keyboard";
	}
	
}


