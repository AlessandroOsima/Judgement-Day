using UnityEngine;
using System.Collections;

public class ToggleEventForwarder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onToggleStateChanged()
	{
		LevelGUI.levelGUI.onPowerButtonPressed (this.GetComponent<UIToggle>());
	}
}
