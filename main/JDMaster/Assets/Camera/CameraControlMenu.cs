using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class CameraControlMenu : MonoBehaviour
{
	public float speed = 90;
	public float horizontalMovement = 100;
	public float rayLenght = 1.5f;
	public float tweenDuration = 1f;
	public List<Vector3> positions;
	
	
	float distFromMin;
	float distFromMax;
	
	static TweenPosition tweenPos;
	private int selectedIsland = 0;
	int islandsNumber = 0;
	bool running = false;
	
	Vector3 direction;
	Vector3 origin;
	Vector3 movement;
	Vector3 destination;
	
	int moveDirection = 0;
	
	void Start()
	{
		tweenPos = camera.GetComponent<TweenPosition>();
		tweenPos.ignoreTimeScale = true;
		tweenPos.duration = tweenDuration;
		tweenPos.style = UITweener.Style.Once;
		tweenPos.method = UITweener.Method.EaseInOut;
		islandsNumber = positions.Count;
	}
	
	void Update()
	{
		if (running) return;
		
		direction = new Vector3(0, 0, 0);
		origin = transform.position;
		
		float movement = Input.GetAxis("Horizontal");
		Debug.Log(movement);
		
		if (movement == 0) return;
		else if (movement > 0)
		{
			++selectedIsland;
			
			if (selectedIsland > islandsNumber - 1)
			{
				selectedIsland--;
				return;
			}
		}
		else if (movement < 0)
		{
			--selectedIsland;
			
			if (selectedIsland < 0) selectedIsland = 0;
		}
		running = true;
		tweenPos.from = origin;
		tweenPos.to = positions[selectedIsland];
		tweenPos.ResetToBeginning();
		tweenPos.onFinished.Clear();
		EventDelegate.Add(tweenPos.onFinished, OnTweenFinished);
		tweenPos.PlayForward();
		
	}
	
	void OnTweenFinished()
	{
		running = false;
	}
	
	public static TweenPosition cameraTweenPosition
	{
		get
		{
			return tweenPos;
		}
	}
	
}
