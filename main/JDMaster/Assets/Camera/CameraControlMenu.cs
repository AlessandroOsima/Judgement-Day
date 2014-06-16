using System.Collections.Generic;
using UnityEngine;

public class CameraControlMenu : MonoBehaviour
{
	public enum LevelCorner
	{
		Left,
		Right,
		None
	}
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
	LevelCorner corner = LevelCorner.None;
	int numberOfIsles = 0;
	bool running = false;

	IslandDescriptor currentIsland;
	IslandDescriptor previousIsland;

	public delegate void onIslandTransition(IslandDescriptor previousLevel, IslandDescriptor newLevel, LevelCorner corner);
	public delegate void onLevelSelected(IslandDescriptor level);

	public static List<GameObject> staticPositions;
	public event onLevelSelected levelSelected;
	public event onIslandTransition islandTransitionStart;
	public event onIslandTransition islandTransitionEnd;


	Vector3 direction;
	Vector3 origin;
	Vector3 movement;
	Vector3 destination;

	int moveDirection = 0;

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

	void Start()
	{

		Debug.Log (InputMapping.getValidControllersCount ());

		foreach (var ID in InputMapping.getValidControllersID ())
						Debug.Log (ID);

		InputMapping.useController (1);

		selectedIsland = 0;
		this.transform.position = new Vector3(islands[0].transform.position.x,this.transform.position.y,islands[0].transform.position.z);
		selectors[0].SetActive(true);
		tweenPos = camera.GetComponent<TweenPosition>();
		tweenPos.ignoreTimeScale = true;
		tweenPos.duration = tweenDuration;
		tweenPos.style = UITweener.Style.Once;
		tweenPos.method = UITweener.Method.EaseInOut;
		numberOfIsles = positions.Count;
		staticPositions = islands;
		currentIsland = islands[selectedIsland].GetComponent<IslandDescriptor>();
		previousIsland = null;

		if(islandTransitionEnd != null)
			islandTransitionEnd(previousIsland,currentIsland,LevelCorner.Left);
	}

	void Update()
	{
		if (running) return;

		direction = new Vector3(0, 0, 0);
		origin = transform.position;


		float movement = InputMapping.GetAction (Actions.Horizontal);  //Input.GetAxis("Horizontal");
//		Debug.Log (movement);

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
			selectors[selectedIsland].SetActive(false);
			--selectedIsland;

			if (selectedIsland < 0) 
			{
				selectedIsland = 0;
				return;
			}

			selectors[selectedIsland + 1].SetActive(false);
		}

		if(selectedIsland == 0)
			corner = LevelCorner.Left;
		else if(selectedIsland == numberOfIsles -1 )
			corner = LevelCorner.Right;
		else
			corner = LevelCorner.None;

		running = true;
		tweenPos.from = origin;
		tweenPos.to = new Vector3(islands[selectedIsland].transform.position.x,origin.y,islands[selectedIsland].transform.position.z);
		tweenPos.ResetToBeginning();
		tweenPos.onFinished.Clear();

		previousIsland = currentIsland;
		currentIsland = islands[selectedIsland].GetComponent<IslandDescriptor>();


		if(islandTransitionStart != null)
			islandTransitionStart(previousIsland,currentIsland,corner);

		EventDelegate.Add(tweenPos.onFinished, OnTweenFinished);
		tweenPos.PlayForward();

	}

	void OnTweenFinished()
	{
		selectors[selectedIsland].SetActive(true);
		running = false;

		if(islandTransitionEnd != null)
		 islandTransitionEnd(previousIsland,currentIsland,corner);
	}

    //To be Called when a level is selected only by ChangeSecen, a bit hacky, will have to replace this
    public void notifyLevelSelected()
    {
        levelSelected(currentIsland);
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        