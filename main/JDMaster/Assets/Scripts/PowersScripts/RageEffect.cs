using UnityEngine;
using System.Collections;

public class RageEffect : PowerEffect 
{
	
	float timer = 0;
	bool started = false;
	bool overrideOnTriggerEnter = false;

	//UPDATE POWER EFFECTS
	public override void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator)
	{
		if(status.UnitStatus != PersonStatus.Status.Raged && status.UnitStatus != PersonStatus.Status.Dead)
		{
			if(animator.powerEffect)
				Object.Destroy(animator.powerEffect);

			timer = 0;
			status.UnitStatus = PersonStatus.Status.Raged;
			navigator.Stop();
			animator.powerEffect = (GameObject) Object.Instantiate(owner.particleEffect, new Vector3(status.transform.position.x,status.transform.position.y + 0.9f,status.transform.position.z),Quaternion.identity);
			animator.powerEffect.transform.parent = animator.transform;
			status.Fear = owner.Fear;
		}
		else
		{
			timer += Time.deltaTime;
			if (timer >= 10f)
			{
				status.UnitStatus = PersonStatus.Status.Concerned;
				status.Fear = 0;
				Object.Destroy(animator.powerEffect);
				status.ActivePower = null;
				navigator.SetNewPatrolDestination(navigator.Type);
				navigator.target = null;
			}
		}
		
		if(status.UnitStatus == PersonStatus.Status.Dead)
		{
			if(animator.powerEffect != null)
				Object.Destroy(animator.powerEffect);
		}
	}
	
	//NAVIGATION
	void SetTarget(GameObject current, UnitNavigationController navigator)
	{
		GameObject[] Persons = GameObject.FindGameObjectsWithTag(GlobalManager.npcsTag);
		float closestDist = Mathf.Infinity;
		int closest = -1;
		
		if(Persons.Length > 0)
		{
			/*
			for (int c = 0; c < Persons.Length; c++)
			{
				Debug.Log(Persons[c]);
			}
			*/

			for (int i = 0; i < Persons.Length; i++)
			{
				/*
				if(Persons == null)
					Debug.Log("Fuck");

				if(Persons[i] == null)
					Debug.Log("Fuck^2");

				if(current == null)
					Debug.Log("Fuck^3");

				if(Persons[i].GetComponent<PersonStatus>() == null)
				{
					Debug.Log(Persons[i]);
					Debug.Log("Fuck^4");
				}

				if(Persons[i].GetComponent<PersonStatus>().isAlive() == null)
					Debug.Log("Fuck^5");
                */

				if (Persons[i] != current && Persons[i].GetComponent<PersonStatus>().isAlive())
				{
					float dist = (navigator.transform.position - Persons[i].transform.position).sqrMagnitude;
					
					if (dist < closestDist)
					{
						closestDist = dist;
						closest = i;
					}
				}
			}
			
			if (closest != -1)
			{
				Debug.Log("Recomputing target");
				navigator.target = Persons[closest].GetComponent<PersonStatus>();
			}
		}
		else
		{
			navigator.target = null;
		}
		
	}

	public override void deliverOnCollisionEffect(Collider other, PersonStatus status)
	{
		if(other != status.gameObject.collider)
		{
			if (other.tag == "Person")
			{
				AnimationScript other_anim = other.GetComponent<AnimationScript>();
				PersonStatus otherPerson = other.GetComponent<PersonStatus>();

				otherPerson.UnitStatus = PersonStatus.Status.Dead;

			}
		}
	}

	public override bool OnTriggerEnterOverride(Collider other, Power owner)
	{
		if(other.tag == GlobalManager.npcsTag)
		{
			PersonStatus person = other.GetComponent<PersonStatus>();
			
			if(person.UnitStatus != PersonStatus.Status.Dead && person.IsAValidTarget)
			{
				//owner.audio.Play();
				
				if(person.ActivePower != null && person.ActivePower.effectName == "calma")
				{
					person.ActivePower = null;
				}
				
				person.ActivePower = this;
			}
			
		}
		return true;
	}
	
	public override void runNavigatorUpdate(UnitNavigationController navigator)
	{
		SetTarget(navigator.gameObject, navigator); 
		
		if(navigator.target == null)
		{
			navigator.SetSpeed(navigator.RunningSpeed);
			navigator.Panicking();
			
		}
		else
		{
			navigator.SetSpeed(navigator.RunningSpeed);
			navigator.SetNavDestination(navigator.target.transform.position);
		}
	}

}
