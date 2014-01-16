using UnityEngine;
using System.Collections;

public class FireEffect : PowerEffect 
{

	float timer = 0;
	bool started = false;

	//UPDATE POWER EFFECTS
	public override void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator)
	{
		if(status.UnitStatus != PersonStatus.Status.Raged && status.UnitStatus != PersonStatus.Status.Dead && !started)
		{
			if(animator.powerEffect)
				Object.Destroy(animator.powerEffect);

			started = true;
			status.UnitStatus = PersonStatus.Status.Panicked;
			animator.powerEffect = (GameObject) Object.Instantiate(owner.particleEffect, new Vector3(status.transform.position.x,status.transform.position.y + 0.9f,status.transform.position.z),Quaternion.identity);
			animator.powerEffect.transform.parent = animator.transform;
			status.Fear = owner.Fear;
		}
		else if(status.UnitStatus != PersonStatus.Status.Raged && status.UnitStatus != PersonStatus.Status.Dead)
		{
			timer += Time.deltaTime;
			if (timer >= 3f)
			{
				status.Fear = 0;
				status.UnitStatus = PersonStatus.Status.Dead;
				navigator.Stop();
				started = false;
			}
		}
		
		if(status.UnitStatus == PersonStatus.Status.Dead)
		{
			Object.Destroy(animator.powerEffect,3f);
			status.ActivePower = null;
		}
	}
	
	//NAVIGATION
	public override void runNavigatorUpdate(UnitNavigationController navigator)
	{
		navigator.Panicking();
		navigator.SetSpeed(navigator.RunningSpeed);
	}

	public override void deliverOnCollisionEffect(Collider other, PersonStatus status)
	{
		if(other.tag != "Person")
			return;

		if(other == status.collider)
			return;

		AnimationScript other_anim = other.GetComponent<AnimationScript>();
		PersonStatus otherPerson = other.GetComponent<PersonStatus>();

		if(otherPerson.ActivePower == null)
		{
			FireEffect effect = new FireEffect();
			effect.initialize(owner);
			otherPerson.ActivePower = effect;
		}
	}

	public override bool OnTriggerEnterOverride(Collider other, Power owner)
	{
		if(other.tag == GlobalManager.npcsTag)
		{
			PersonStatus person = other.GetComponent<PersonStatus>();

			if(person.UnitStatus != PersonStatus.Status.Dead && person.IsAValidTarget)
			{
				owner.audio.Play();

				if(person.ActivePower != null && person.ActivePower.effectName == "calma")
				{
					person.ActivePower = null;
				}

				person.ActivePower = this;
			}
			
		}
		
		return true;
	}
}
