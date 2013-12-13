﻿using UnityEngine;
using System.Collections;

public class Rage : Power 
{
	public float speed = 25f;
	public int Fear = 70;
	public int Highness = 0;
	public GameObject godRay;
	public GameObject particleEffect;
	bool ready = true;
	int souls;
	Vector3 startPosition;
	Vector3 Location; //for the preview of the power
	public float distance = 40;
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
		if(status.UnitStatus != PersonStatus.Status.Raged)
		{
			animator._burnTimer = 0;
			status.UnitStatus = PersonStatus.Status.Raged;
			animator.powerEffect = (GameObject)Instantiate(particleEffect, new Vector3(status.transform.position.x,status.transform.position.y + 0.9f,status.transform.position.z),Quaternion.identity);
			animator.powerEffect.transform.parent = animator.transform;
			status.Fear = Fear;
		}
		else
		{
			animator._burnTimer += Time.deltaTime;
			if (animator._burnTimer >= 10f)
			{
				status.UnitStatus = PersonStatus.Status.Concerned;
				status.Fear = 0;
				Destroy(animator.powerEffect);
				status.ActivePower = null;
				navigator.SetNewDestination(navigator.Type);
			}
		}
	}

	public override void OnTriggerEnter(Collider other)
	{
		if(other.tag == GlobalManager.npcsTag)
		{
			PersonStatus person = other.GetComponent<PersonStatus>();

			if(person.UnitStatus != PersonStatus.Status.Dead)
			{
				audio.Play();
				person.ActivePower = this;
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
