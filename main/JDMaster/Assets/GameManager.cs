using UnityEngine;
using System.Collections;

//Where all the global game vars are (will be) located. This is against every programming rule..... fuck them.
public static class GameManager 
{
	//InputMapping mapper;
	static string nextScene;

	public static string NextScene
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
}
