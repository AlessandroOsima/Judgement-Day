using UnityEngine;
using System.Collections;

public class CalmEffect : PowerEffect 
{
	
	float timer = 0;
	bool started = false;
	bool overrideOnTriggerEnter = true;

	public override void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator)
	{
		if(status.UnitStatus != PersonStatus.Status.Dead)
		{
			status.UnitStatus = PersonStatus.Status.Calm;
			status.Fear = 0;
			navigator.SetNewPatrolDestination(navigator.Type);
			
			if(animator.powerEffect)
				Object.Destroy(animator.powerEffect);
			
			animator.powerEffect =(GameObject) Object.Instantiate(owner.particleEffect, new Vector3(status.transform.position.x,status.transform.position.y + 0.9f,status.transform.position.z),Quaternion.identity);
			animator.powerEffect.transform.parent = animator.transform;
			Object.Destroy(animator.powerEffect,5.0f);
			status.ActivePower = null;
		}
	}
	
	public override void runNavigatorUpdate(UnitNavigationController navigator)
	{
	}

	public override bool OnTriggerEnterOverride(Collider other, Power owner)
	{
		if(other.tag == GlobalManager.npcsTag)
		{
			PersonStatus person = other.GetComponent<PersonStatus>();
			
			if(person.UnitStatus != PersonStatus.Status.Dead && person.UnitStatus != PersonStatus.Status.Raged && person.IsAValidTarget)
			{
				owner.audio.Play();
				person.ActivePower = this;
			}
			
			if(person.UnitStatus != PersonStatus.Status.Dead && person.UnitStatus == PersonStatus.Status.Raged && person.IsAValidTarget)
			{
				owner.audio.Play();
				person.ActivePower = null;
				person.ActivePower = this;
			}
			
		}

		return true;
	}

	public override void deliverOnCollisionEffect(Collider other, PersonStatus status)
	{
		
	}
}
