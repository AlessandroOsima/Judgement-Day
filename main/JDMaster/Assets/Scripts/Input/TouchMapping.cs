using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchController : ActionController
{
	private Vector2 firstPressPos, secondPressPos;
	private Vector2 currentSwipe;
    private Vector2 pinchFinger1, pinchFinger2;
    private Vector3 tapPosition;
    private float zoom=0f;

	public override void Start()
	{
		
	}

	public override bool useOnPlatform(string controllerName)
	{

		var os = SystemInfo.operatingSystem;
		
		//Debug.Log (os);
		
		if (os.Contains ("Windows Phone")) 
			return true;


		return false;
	}

    private float Tap()
    {
        if (Input.touches.Length == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began && t.tapCount == 1)
            {
                firstPressPos = new Vector2(t.position.x, t.position.y);
                return 0;
            }
            
            if (t.phase == TouchPhase.Ended && t.tapCount == 1)
            {
                tapPosition = t.position;

                //if (tapPosition.x < 5)
                //   return 0;
                if(firstPressPos == new Vector2(t.position.x, t.position.y))
                    
                    return 1.0f;
            }
        }
        return 0;
    }
    

	private float Swipe(string axis)
	{
		if(Input.touches.Length==1)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began)
			{
				firstPressPos = new Vector2(t.position.x,t.position.y);
				return 0;

			}
            if (t.phase == TouchPhase.Stationary)
            {
                return 0;
            }
            if (t.phase == TouchPhase.Moved)
            {
                secondPressPos = new Vector2(t.position.x, t.position.y);
                if (Vector2.Distance(secondPressPos, firstPressPos) < 50f)
                    return 0;
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y, 0);
                currentSwipe.Normalize();
            }
			if(t.phase == TouchPhase.Ended)
			{
                return 0;
			}
			if(axis.Equals("x"))
				return -currentSwipe.x;
			if(axis.Equals("y"))
				return -currentSwipe.y;
			return 0;
		}else{
			return 0;
		}
	}

    private float Pinch()
    {
        if (Input.touchCount == 2)
        {
            var finger1 = Input.GetTouch(0);
            var finger2 = Input.GetTouch(1);

            if (finger1.phase == TouchPhase.Began && finger2.phase == TouchPhase.Began)
            {
                this.pinchFinger1 = new Vector2(finger1.position.x, finger1.position.y);
                this.pinchFinger2 = new Vector2(finger2.position.x, finger2.position.y);
                return 0;
            }
            if (finger1.phase == TouchPhase.Stationary && finger2.phase == TouchPhase.Stationary)
            {
                zoom = 0;
                return 0;
            }
            
            if (finger1.phase == TouchPhase.Ended || finger2.phase == TouchPhase.Ended)
            {
                zoom = 0;
                return 0;
            }
            if (finger1.phase == TouchPhase.Moved || finger2.phase == TouchPhase.Moved)
            {
                float baseDistance = Vector2.Distance(this.pinchFinger1, this.pinchFinger2);
                float currentDistance = Vector2.Distance(finger1.position, finger2.position);
                float distancePercent = currentDistance / baseDistance;
                if (distancePercent > 1.0f && zoom >=0)
                {
                    zoom = 1.0f;
                    return zoom;
                }
                if (distancePercent < 1.0f && zoom <= 0)
                {
                    zoom = -1.0f;
                    return zoom;
                }
            }
        }
        return 0;
    }


	public override float GetAction(Actions action)
	{
		
		switch(action)
		{
		case Actions.Horizontal :
		{
			return Swipe("x");
		}
			
		case Actions.Vertical :
		{
			return Swipe("y");
		}
			
		case Actions.Zoom :
		{
			return Pinch();
		}
			
		case Actions.Use :
		{
			return Tap();
		}

		case Actions.Skip :
		{
            return Tap();
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
			
		default :
		{
			#if DEBUG
			Debug.LogWarning("No touch gesture registered in touch controller for Action : " + action);
			#endif
			
			return 0.0f;
		}
		}
	}

    public override Vector3 GetActionPosition(Actions action)
    {
        if (action == Actions.Use)
        {
            Debug.Log(tapPosition);

            if (tapPosition.x > 125)
                return tapPosition;
            else
                return Vector3.zero;
        }
        return Vector3.zero;
    }
	
	public override string ID()
	{
		return "Touch";
	}
	
}


