using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public int health = 5;
	bool hasForce;
	public Force script;
	// Use this for initialization
	void Start () {
		script = GetComponent<Force>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			hasForce = script.ReportForce();
			if(hasForce)health = 10;
		}
	}
}
