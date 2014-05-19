using UnityEngine;
using System.Collections;

public class DescriptionUpdater : MonoBehaviour {

	UILabel description;

	// Use this for initialization
	void Awake () 
	{
		description = this.GetComponent<UILabel>();
		var cameraCo = FindObjectOfType<CameraControlMenu>();
		cameraCo.islandTransitionStart += onIslandTransitionStart;
		cameraCo.islandTransitionEnd += onIslandTransitionEnd;
		cameraCo.levelSelected += onLevelSelected;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void onLevelSelected(IslandDescriptor level)
	{
		description.enabled = false;
	}
	
	void onIslandTransitionStart(IslandDescriptor previousLevel, IslandDescriptor newLevel, CameraControlMenu.LevelCorner corner)
	{
		description.enabled = false;
	}

	void onIslandTransitionEnd(IslandDescriptor previousLevel, IslandDescriptor newLevel, CameraControlMenu.LevelCorner corner)
	{
		description.enabled = true;
		description.text = newLevel.Description;
	}
}
