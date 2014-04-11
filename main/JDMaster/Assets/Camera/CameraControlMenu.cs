using System.Collections.Generic;
using UnityEngine;

public class CameraControlMenu : MonoBehaviour
{
	public float speed = 90;
	public float horizontalMovement = 100;
	public float rayLenght = 1.5f;
	public float tweenDuration = 1f;
	public List<Vector3> positions;
	public List<GameObject> islands;
	public List<GameObject> selectors;
	
	float distFromMin;
	float distFromMax;
	
	static TweenPosition tweenPos;
	static int selectedIsland = 0;
	int numberOfIsles = 0;
	bool running = false;
	public static List<GameObject> staticPositions;

	Vector3 direction;
	Vector3 origin;
	Vector3 movement;
	Vector3 destination;
	
	int moveDirection = 0;
	
	void Start()
	{
		this.transform.position = new Vector3(islands[0].transform.position.x,this.transform.position.y,islands[0].transform.position.z);
		selectors[0].SetActive(true);
		tweenPos = camera.GetComponent<TweenPosition>();
		tweenPos.ignoreTimeScale = true;
		tweenPos.duration = tweenDuration;
		tweenPos.style = UITweener.Style.Once;
		tweenPos.method = UITweener.Method.EaseInOut;
		numberOfIsles = positions.Count;
		staticPositions = islands;
	}
	
	void Update()
	{
		if (running) return;
		
		direction = new Vector3(0, 0, 0);
		origin = transform.position;

		float movement = Input.GetAxis("Horizontal");
		
		if (movement == 0) 
			return;
		else if (movement > 0)
		{
			++selectedIsland;
			
			if (selectedIsland > numberOfIsles - 1)
			{
				selectedIsland--;
				return;
			}

			selectors[selectedIsland - 1].SetActive(false);
		}
		else if (movement < 0)
		{
			--selectedIsland;
			
			if (selectedIsland < 0) 
			{
				selectedIsland = 0;
				return;
			}

			selectors[selectedIsland + 1].SetActive(false);

		}

		running = true;
		tweenPos.from = origin;
		tweenPos.to = new Vector3(islands[selectedIsland].transform.position.x,origin.y,islands[selectedIsland].transform.position.z);
		tweenPos.ResetToBeginning();
		tweenPos.onFinished.Clear();
		EventDelegate.Add(tweenPos.onFinished, OnTweenFinished);
		tweenPos.PlayForward();
		
	}
	
	void OnTweenFinished()
	{
		selectors[selectedIsland].SetActive(true);
		running = false;
	}
	
	#region Properties
	public static TweenPosition cameraTweenPosition
	{
		get
		{
			return tweenPos;
		}
	}


	public bool isRunning
	{
		get
		{
			return running;
		}

		set
		{
			running = value;
		}
	}

	public static List<GameObject> IslandsPositions
	{
		get
		{
			return staticPositions;
		}
	}
	
	public int SelectedIsle
	{
		get
		{
			return selectedIsland;
		}
	}

	#endregion
	
}

