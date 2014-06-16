using UnityEngine;
using System.Collections;

public class CreditsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (InputMapping.GetAction(Actions.Quit) > 0 || InputMapping.GetAction(Actions.Skip) > 0)
			Application.Quit();
	
	}

	public void OnTweenCompleted()
	{
		Application.Quit();
	}
}
