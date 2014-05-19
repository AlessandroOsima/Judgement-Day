using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuGUI : MonoBehaviour 
{
	public void OnClick()
	{
		Application.LoadLevel("Abouts");
	}

	void Update()
	{
		if(InputMapping.GetAction(Actions.Quit) > 0)
			Application.LoadLevel("Abouts");
	}
}
	