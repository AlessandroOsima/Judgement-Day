using UnityEngine;
using System.Collections;

public class Lighting : Power 
{
	public float speed = 25f;
	public int Fear = 70;
	public int Highness = 20;
	public GameObject power;
	bool ready = true;
	int souls;
	Vector3 startPosition;
	Vector3 Location; //for the preview of the power
	public float distance = 40;
	
	// Use this for initialization
	void Start () {
		startPosition = transform.position; //saves the start position
		//pscore = GameObject.Find("_Global").GetComponent<Player_Score>(); //initializing the script
		//stat = GameObject.Find("Human").GetComponent<Status>();
	}

	void OnEnable() //The Power is activated by the power manager, initialize stuff here, do NOT use any find* function (Slooow !!)
	{
		power.SetActive(true);
		ready = true;
	}
	
	void OnDisable() //The Power is deactivated by the power manager, deinitialize stuff here, do NOT use any find* function
	{
		power.SetActive(false);
	}

	public override void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator)
	{
		status.UnitStatus = PersonStatus.Status.Shocked;
		status.Fear = this.Fear;
		status.ActivePower = null;
	}

	public override void OnTriggerEnter(Collider other) 
	{
		if(other.tag == GlobalManager.npcsTag)
		{
			PersonStatus person = other.GetComponent<PersonStatus>();

			if(person.UnitStatus != PersonStatus.Status.Dead)
				person.ActivePower = this;
		}
	}

	public override void runNavigatorUpdate(UnitNavigationController navigator)
	{
	}

	// Update is called once per frame
	void Update () 
	{
		souls = GlobalManager.globalManager.souls;

		if(ready)
		{
			//PREVIEW
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Location = ray.origin + (ray.direction * distance);
			Location.y = Highness;
			transform.position = Location;


			if(Input.GetMouseButton(0))
			{ 
				Debug.Log("HIT for :" + this.name);
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
					this.transform.position = Location;
					GlobalManager.globalManager.decrementSouls(price);

			}
		}
	
	 }
}
