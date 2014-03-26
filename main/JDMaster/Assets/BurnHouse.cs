using UnityEngine;
using System.Collections;

public class BurnHouse : ValidTarget 
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
			if(canBeTargeted)
			{
				canBeTargeted = false;
			}

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

		if(this.name == "HouseA01Base")
		{
			gameObject.transform.FindChild("HouseA01Roof").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseA01Wall").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseA01Wood").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
		}

		if(this.name == "HouseA02Base")
		{
			gameObject.transform.FindChild("HouseA02Roof").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseA02Wall").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseA02Wood").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
		}

		if(this.name == "HouseB01Base")
		{
			gameObject.transform.FindChild("HouseB01Roof").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseB01Wall").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseB01Wood").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
		}

		if(this.name == "HouseB02Base")
		{
			gameObject.transform.FindChild("HouseB02Roof").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseB02Wall").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseB02Wood").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
		}

		if(this.name == "HouseStone01Base")
		{
			gameObject.transform.FindChild("HouseStone01Door").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseStone01Roof").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
		}

		if(this.name == "HouseStoneBase")
		{
			gameObject.transform.FindChild("HouseStone01Door").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseStone02Roof").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
		}

		if(this.name == "EasterHouse")
		{
			gameObject.transform.FindChild("HouseCeiling06").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
			gameObject.transform.FindChild("HouseWood06").gameObject.renderer.materials[0].SetFloat("_Burn",burnLevel);
		}

	}

	void burnPerson(PersonStatus status)
	{
		FireEffect effect = new FireEffect();
		effect.initialize(fireDealer);
		status.ActivePower = effect;
	}

}
