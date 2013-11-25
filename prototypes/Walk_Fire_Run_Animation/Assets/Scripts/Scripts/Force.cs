using UnityEngine;
using System.Collections;

public class Force : MonoBehaviour {
	private bool force;
	// Use this for initialization
	void Start () {
		force = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P))
			print("I have" + (force?" the":" no") + " force");
	}
	
	public bool ReportForce(){
		return force;
	}
}
