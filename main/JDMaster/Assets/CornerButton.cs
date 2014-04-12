using UnityEngine;
using System.Collections;

public class CornerButton : MonoBehaviour 
{
	public bool left = false;
	UISprite button;

	// Use this for initialization
	void Awake () 
	{
		var cameraCo = FindObjectOfType<CameraControlMenu>();
		cameraCo.islandTransitionStart += onIslandTransitionStart;
		cameraCo.islandTransitionEnd += onIslandTransitionEnd;
		cameraCo.levelSelected += onLevelSelected;

		button = this.GetComponent<UISprite>();
	}

	void onIslandTransitionStart(IslandDescriptor previousLevel, IslandDescriptor newLevel, CameraControlMenu.LevelCorner corner)
	{
		//button.enabled = false;
	}

	void onLevelSelected(IslandDescriptor level)
	{
		button.enabled = false;
	}

	void onIslandTransitionEnd(IslandDescriptor previousLevel, IslandDescriptor newLevel, CameraControlMenu.LevelCorner corner)
	{

		if(corner == CameraControlMenu.LevelCorner.Left && left)
			button.enabled = false;

		if(corner == CameraControlMenu.LevelCorner.Right && !left)
			button.enabled = false;

		if (corner == CameraControlMenu.LevelCorner.None)
			button.enabled = true;
	}
}
