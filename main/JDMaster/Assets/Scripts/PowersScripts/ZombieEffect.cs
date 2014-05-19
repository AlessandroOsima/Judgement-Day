using UnityEngine;
using System.Collections;

public class ZombieEffect : PowerEffect 
{
	
	float timer = 0;
	bool started = false;
	bool overrideOnTriggerEnter = false;
	float aliveTime = 20f;
	float speed = 4.0f;
	float halfSpeed;
	float thirdSpeed;
	bool stop = false;

	//UPDATE POWER EFFECTS
	public override void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator)
	{
		if(timer == 0)
		{
			if(animator.powerEffect)
				Object.Destroy(animator.powerEffect);

			var renderers = status.gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>();

			foreach(var renderer in renderers)
			{
				if(renderer.gameObject.name == "Character01")
				{
					renderer.material.mainTexture = (Texture2D)Resources.Load("Character01Zombie",typeof(Texture2D));
				}
			}

			status.UnitStatus = PersonStatus.Status.Zombie;
			navigator.Stop();
			halfSpeed = speed/2;
			thirdSpeed = speed/3;
			animator.powerEffect = (GameObject) Object.Instantiate(owner.particleEffect, new Vector3(status.transform.position.x,status.transform.position.y + 0.9f,status.transform.position.z),Quaternion.identity);
			animator.powerEffect.transform.parent = animator.transform;
			status.Fear = owner.Fear;
		}

		timer += Time.deltaTime;

		if(timer >= (aliveTime * 0.25f) && timer <= (aliveTime * 0.75f) && !stop)
		{
			//Debug.Log(speed);
			speed = halfSpeed;
			animator.SetSpeed(animator.MiddleSpeed);
		}

		/*
		if(timer >= (aliveTime * 0.5f) && timer <= (aliveTime * 0.75f))
		{
			Debug.Log(speed);
			speed = speed / 2;
		}
		*/

		if(timer >= (aliveTime * 0.75f) && !stop)
		{
			//Debug.Log(speed);
			speed = thirdSpeed;
			animator.SetSpeed(0.5f);
			animator.SetSpeed(animator.WalkingSpeed);
		}

		if(stop)
		{
			animator.SetSpeed(0);
		}

		if (timer >= aliveTime)
		{
			status.UnitStatus = PersonStatus.Status.Dead;
			status.Fear = 0;
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
				PersonStatus status = Persons[i].GetComponent<PersonStatus>();

				if (Persons[i] != current && status.isAlive() && status.UnitStatus != PersonStatus.Status.Zombie)
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
				//Debug.Log("Recomputing target");
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

				if(!otherPerson.isAlive() || otherPerson.UnitStatus == PersonStatus.Status.Zombie)
					return;

				otherPerson.ActivePower = null;
				ZombieEffect effect = new ZombieEffect();
				effect.initialize(owner);
				otherPerson.ActivePower = effect;
			}
		}
	}

	public override bool OnTriggerEnterOverride(Collider other, Power owner)
	{
		if(other.tag == GlobalManager.npcsTag)
		{
			PersonStatus person = other.GetComponent<PersonStatus>();
			
			if(person.UnitStatus != PersonStatus.Status.Dead && person.IsAValidTarget && person.UnitStatus != PersonStatus.Status.Zombie)
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
			navigator.Stop();
			stop = true;
		}
		else
		{
			navigator.SetNavDestination(navigator.target.transform.position);
		}

		if(!stop)
		navigator.SetSpeed(speed);

	}

}
