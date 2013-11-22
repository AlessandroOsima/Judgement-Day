using UnityEngine;
using System.Collections;

public class PowerRain : MonoBehaviour {
	public float speed = 25f;
	public Player_Score pscore; //declaring the script
	//public Status stat;
	bool pressedR = false;
	bool ready = false;
	int souls;
	Vector3 startPosition;
	Vector3 Location; //for the preview of the power
	public float distance = 10f;
	// Use this for initialization
	void Start () {
		startPosition = transform.position; //saves the start position
		pscore = GameObject.Find("_Global").GetComponent<Player_Score>(); //initializing the script
		//stat = GameObject.Find("Human").GetComponent<Status>();
	}
	
	// Update is called once per frame
	void Update () {
		souls = pscore.reportSouls();
		if((souls >= 5) && (Input.GetKeyDown(KeyCode.R)) && !ready){ //if you have enough souls and want to do the rain power
			pressedR = true;
		}
		if(pressedR){
			//PREVIEW
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Location = ray.origin + (ray.direction * distance);
			Location.y = 10;
			transform.position = Location;
			if(Input.GetMouseButton(0)){ //if pressed the left button it takes the coord for the power to take place
				pscore.decrementSouls(5);
				pressedR = false;
				ready = true;
			}
			if(Input.GetMouseButton(1)){ //if pressed the right button cancels the power
				pressedR = false;
			}
		}

		if(ready){
			transform.position = transform.position + Vector3.down*speed*Time.deltaTime;
			if(transform.position.y < -20){
				ready = false;
				transform.position = startPosition;
			}
		}
	}
}
