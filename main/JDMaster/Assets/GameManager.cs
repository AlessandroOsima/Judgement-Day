using UnityEngine;
using System.Collections;

//Where all the global game vars are (will be) located. This is against every programming rule..... fuck them.
public class GameManager : MonoBehaviour 
{
	//InputMapping mapper;
	string nextScene;

	/*
	public InputMapping inputMapping
	{
		get
		{
			return mapper;
		}
	}
	*/

	public string NextScene
	{
		get
		{
			return nextScene;
		}

		set
		{
			nextScene = value;
		}
	}

	// Use this for initialization
	void Awake () 
	{
		//mapper = new InputMapping();
		//mapper.RegisterControllers ();

		//I will survive for ever !!
		Object.DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
