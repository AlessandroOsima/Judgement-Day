﻿using UnityEngine;
using System.Collections;

public class BurnHouse : MonoBehaviour 
{

	// Use this for initialization
	bool isBurning = false;
	float burnAmount = 0;
	public float burnSpeed = 0.000015f;
	float timer;
	BasePowerDealer fireDealer;
	public GameObject fireHouse;

	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isBurning)
		{
			burnAmount += (Time.deltaTime * burnSpeed);

			timer += Time.deltaTime;

			if(burnAmount >= 1)
				burnAmount = 1;

			if(burnAmount < 1)
			burnToFloat(burnAmount);

			/*
			if(timer >= 10f)
			{
				fireHouse.SetActive(false);
			}
			*/
		}
	}

	public void OnTriggerStay(Collider other) 
	{
		burnCalculations(other);
	}

	public void OnTriggerExit(Collider other) 
	{
		burnCalculations(other);
	}

	public void OnTriggerEnter(Collider other) 
	{
		burnCalculations(other);
	}

	void burnCalculations(Collider other)
	{
		if(other.name == "Fireball" && !isBurning)
		{
			isBurning = true;
			fireHouse.SetActive(true);
			fireDealer = (BasePowerDealer)other.transform.parent.GetComponent<BasePowerDealer>();
		}

		if(other.tag == GlobalManager.npcsTag && !isBurning)
		{
			PersonStatus status = other.GetComponent<PersonStatus>();

			if(status.ActivePower != null && status.ActivePower.effectName == "fuoco")
			{
				isBurning = true;
				fireHouse.SetActive(true);
				fireDealer = (BasePowerDealer)GameObject.Find("Fire").GetComponent<BasePowerDealer>();
			}
		}
		
		if(other.tag == GlobalManager.npcsTag && isBurning)
		{
			PersonStatus status = other.GetComponent<PersonStatus>();

			if(status.UnitStatus == PersonStatus.Status.Dead)
				return;

			if(status.ActivePower == null)
			{
				burnPerson(status);
			}
			else if (status.ActivePower.effectName == "calma" || status.UnitStatus == PersonStatus.Status.Raged)
			{
				status.ActivePower = null;

				if(status.UnitStatus == PersonStatus.Status.Raged)
				{
					status.UnitStatus = PersonStatus.Status.Panicked;
				}

				burnPerson(status);
			}
		}
	}

	void burnToFloat(float burnLevel)
	{
		if(this.name == "HomeStonehenge")
		{
			gameObject.transform.GetChild(1).gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.GetChild(2).gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
		}
	}

	void burnPerson(PersonStatus status)
	{
		FireEffect effect = new FireEffect();
		effect.initialize(fireDealer);
		status.ActivePower = effect;
	}

}
