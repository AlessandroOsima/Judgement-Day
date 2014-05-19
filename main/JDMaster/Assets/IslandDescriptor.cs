using UnityEngine;
using System.Collections;

public class IslandDescriptor : MonoBehaviour 
{
	//Horrible hack, to set the name and description via editor until we have a proper in-editor properties extension.
	public string startDescription;
	public string startName;


	private string islandName;
	private string IslandDescription;


	// Use this for initialization
	void Awake () 
	{
		islandName = startName;
		IslandDescription = startDescription;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public string Name
	{
		get
		{
			return islandName;
		}

	}

	public string Description
	{
		get 
		{
			return IslandDescription;
		}
	}
}
