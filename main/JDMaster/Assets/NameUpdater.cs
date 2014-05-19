using UnityEngine;
using System.Collections;

public class NameUpdater : MonoBehaviour 
{
	UILabel name;

	// Use this for initialization
	void Awake () 
	{
		name = this.GetComponent<UILabel>();

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
		name.enabled = false;
	}

	void onIslandTransitionStart(IslandDescriptor previousLevel, IslandDescriptor newLevel, CameraControlMenu.LevelCorner corner)
	{
		name.enabled = false;
	}

	void onIslandTransitionEnd(IslandDescriptor previousLevel, IslandDescriptor newLevel, CameraControlMenu.LevelCorner corner)
	{
		name.enabled = true;
		name.text = newLevel.Name;
	}
}
