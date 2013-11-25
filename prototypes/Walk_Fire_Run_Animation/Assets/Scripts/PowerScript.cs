using UnityEngine;
using System.Collections;

public class PowerScript : Power {
	public float speed = 25f;
	public int Fear = 70;
	public int Highness = 20;
	public GameObject power;
	bool ready = true;
	int souls;
	Vector3 startPosition;
	Vector3 Location; //for the preview of the power
	public float distance = 10f;

	//public LineRenderer thunderRenderer;
	//public ParticleRenderer fireRenderer;
	//public int SoulCost = 5;
	//public Player_Score pscore;
	//public string Type;
	//bool pressedButton = false;


	// Use this for initialization
	void Start () {
		startPosition = transform.position; //saves the start position
		//pscore = GameObject.Find("_Global").GetComponent<Player_Score>(); //initializing the script
		//stat = GameObject.Find("Human").GetComponent<Status>();
	}

	void OnEnable() //The Power is activated by the power manager, initialize stuff here, do NOT use any find* function (Slooow !!)
	{
		//enable the thunder and fire renderers so that the power can be seen by the player
		//thunderRenderer.enabled = true;
		//fireRenderer.enabled = true;
		power.SetActive(true);
	}
	
	void OnDisable() //The Power is deactivated by the power manager, deinitialize stuff here, do NOT use any find* function
	{
		//disable the thunder and fire renderers so that the power can't be seen by the player
		power.SetActive(false);
		//thunderRenderer.enabled = false;
		//fireRenderer.enabled = false;
	}


	// Update is called once per frame
	void Update () {
		souls = GlobalManager.globalManager.souls;
		if (powerState==PowerState.Active){
			if(ready)
			{
				//PREVIEW
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Location = ray.origin + (ray.direction * distance);
				Location.y = Highness;
				transform.position = Location;

				if(Input.GetMouseButton(0))
				{ 
					ready = false;
					audio.Play();
				}
			}
			
			if(!ready)
			{
				transform.position = transform.position + Vector3.down*speed*Time.deltaTime;
				
				if(transform.position.y <= -20)
				{
					ready = true;
					transform.position = Location;
					GlobalManager.globalManager.decrementSouls(price);
				}
			}
		}
		/*souls = pscore.reportSouls();
		if(pscore.isUsingPower()==false){
			if(Type=="Fire"){
				if((souls >= SoulCost) && (Input.GetKeyDown(KeyCode.F)) && !ready){ //if you have enough souls and want to do the rain power
					pressedButton = true;
					pscore.setUsingPower(true);
				}
			}
			if(Type=="Lightning"){
				if((souls >= SoulCost) && (Input.GetKeyDown(KeyCode.T)) && !ready){ //if you have enough souls and want to do the rain power
					pressedButton = true;
					pscore.setUsingPower(true);
				}
			}
		}

		if(pressedButton){
			//PREVIEW
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Location = ray.origin + (ray.direction * distance);
			Location.y = 10;
			transform.position = Location;
			if(Input.GetMouseButton(0)){ //if pressed the left button it takes the coord for the power to take place
				pscore.decrementSouls(5);
				pressedButton = false;
				ready = true;
				pscore.setUsingPower(false);
			}
			if(Input.GetMouseButton(1)){ //if pressed the right button cancels the power
				pressedButton = false;
				transform.position = startPosition;
				pscore.setUsingPower(false);
			}
		}

		if(ready){
			transform.position = transform.position + Vector3.down*speed*Time.deltaTime;
			if(transform.position.y < -20){
				ready = false;
				transform.position = startPosition;
			}
		}*/
	}
}
