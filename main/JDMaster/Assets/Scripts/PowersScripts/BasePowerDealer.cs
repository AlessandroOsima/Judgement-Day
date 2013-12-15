using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

using UnityEngine;
using System.Collections;

public abstract class PowerEffect 
{
	protected BasePowerDealer owner;
	public void initialize(BasePowerDealer owner)
	{
		this.owner = owner;
	}
	public abstract bool OnTriggerEnterOverride(Collider other, Power owner);
	public abstract void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator);
	public abstract void runNavigatorUpdate(UnitNavigationController navigator);
}

public class BasePowerDealer : Power 
{
	//Public vars
	public float speed = 25f;
	public int Fear = 70;
	public int Highness = 0;
	public GameObject godRay;
	public GameObject particleEffect;
	public PowerEffect powerEffect;
	public float distance = 40;
	//Private vars
	bool ready = true;
	int souls;
	Vector3 startPosition;
	Vector3 Location; //for the preview of the power
	float previousParticlePosY = 0;
	
	// Use this for initialization
	void Start () {
		startPosition = transform.position; //saves the start position
		//pscore = GameObject.Find("_Global").GetComponent<Player_Score>(); //initializing the script
		//stat = GameObject.Find("Human").GetComponent<Status>();
	}

	void OnEnable() //The Power is activated by the power manager, initialize stuff here, do NOT use any find* function (Slooow !!)
	{
		godRay.SetActive(true);
		particleEffect.SetActive(true);
		ready = true;
	}
	
	void OnDisable() //The Power is deactivated by the power manager, deinitialize stuff here, do NOT use any find* function
	{
		godRay.SetActive(false);
		particleEffect.SetActive(false);
	}

	public override void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator)
	{
	}

	public override void runNavigatorUpdate(UnitNavigationController navigator)
	{
	}


	//COLLISION
	public override void OnTriggerEnter(Collider other)
	{
		string effectName = this.gameObject.name + "Effect";

		//Type type = Type.GetType(effectName);
		PowerEffect effect = (PowerEffect)Assembly.GetExecutingAssembly().CreateInstance(effectName);
		effect.initialize(this);



		if(!effect.OnTriggerEnterOverride(other,this))
		{

			if(other.tag == GlobalManager.npcsTag)
			{
				PersonStatus person = other.GetComponent<PersonStatus>();

				if(person.UnitStatus != PersonStatus.Status.Dead)
				{
					audio.Play();
					person.ActivePower =  effect;
				}
			}
		}
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
			Location.y = 0;
			transform.position = Location;


			if(Input.GetMouseButton(0))
			{ 
				Debug.Log("HIT for :" + this.name);
				ready = false;
				previousParticlePosY = particleEffect.transform.position.y;
			}
		}
			

		if(!ready)
		{

			particleEffect.transform.position = particleEffect.transform.position + Vector3.down*speed*Time.deltaTime;

			if(particleEffect.transform.position.y <= 0)
			{
				ready = true;
				particleEffect.transform.position = new Vector3(Location.x,previousParticlePosY,Location.z);
				GlobalManager.globalManager.decrementSouls(price);
			}
		}
	
	 }
}
