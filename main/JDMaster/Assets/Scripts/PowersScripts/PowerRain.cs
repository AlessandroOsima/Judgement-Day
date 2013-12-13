using UnityEngine;
using System.Collections;

public class PowerRain : Power 
{
	//Public Variables
	public float distance = 10f;
	//The power's particle effects renderers
	public LineRenderer thunderRenderer;
	public ParticleRenderer fireRenderer;
	public float speed = 35f;
	//Private Variables
	bool ready = true;
	int souls;
	Vector3 startPosition;
	//for the preview of the power
	Vector3 Location;
	
	void Start() //also good for initializing stuff, it's better not to set up any event listener here, use OnEnable/OnDisable for that.
	{
		startPosition = transform.position; //saves the start position
	}

	void OnEnable() //The Power is activated by the power manager, initialize stuff here, do NOT use any find* function (Slooow !!)
	{
		//enable the thunder and fire renderers so that the power can be seen by the player
		thunderRenderer.enabled = true;
		fireRenderer.enabled = true;
	}

	void OnDisable() //The Power is deactivated by the power manager, deinitialize stuff here, do NOT use any find* function
	{
		//disable the thunder and fire renderers so that the power can't be seen by the player
		thunderRenderer.enabled = false;
		fireRenderer.enabled = false;
	}

	public override void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator)
	{
		
	}

	public override void OnTriggerEnter(Collider other)
	{
	}
	
	void Update () 
	{
		souls = GlobalManager.globalManager.souls;

		if(ready)
		{
			//PREVIEW
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Location = ray.origin + (ray.direction * distance);
			Location.y = 10;
			transform.position = Location;


			if(Input.GetMouseButton(0))
			{ 
				ready = false;
			}
		}

		if(!ready)
		{
			transform.position = transform.position + Vector3.down*speed*Time.deltaTime;

			if(transform.position.y < -20)
			{
				ready = true;
				transform.position = Location;
				GlobalManager.globalManager.decrementSouls(price);
			}
		}
	}


}
