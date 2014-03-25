using UnityEngine;
using System.Collections;

public class LightingEffect : PowerEffect 
{
	
	float timer = 0;
	bool started = false;
	bool overrideOnTriggerEnter = false;

	//UPDATE POWER EFFECTS
	public override void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator)
	{
		if(status.UnitStatus != PersonStatus.Status.Raged && status.UnitStatus != PersonStatus.Status.Dead && !started)
		{
			if(animator.powerEffect)
				Object.Destroy(animator.powerEffect);

			started = true;
			status.UnitStatus = PersonStatus.Status.Panicked;
			BasePowerDealer fireDealer = (BasePowerDealer) GameObject.Find("Fire").GetComponent<BasePowerDealer>();
			Debug.Log(fireDealer.particleEffect);
			animator.powerEffect = (GameObject) Object.Instantiate(owner.particleEffect, new Vector3(status.transform.position.x,status.transform.position.y + 0.9f,status.transform.position.z),Quaternion.identity);
			animator.powerEffect.transform.parent = animator.transform;   
			status.Fear = owner.Fear;
			status.UnitStatus = PersonStatus.Status.Dead;
		}
		
		if(status.UnitStatus == PersonStatus.Status.Dead)
		{
			Object.Destroy(animator.powerEffect,1f);
			status.ActivePower = null;
		}
	}

	public override void runNavigatorUpdate(UnitNavigationController navigator)
	{
		navigator.Panicking();
		navigator.SetSpeed(navigator.RunningSpeed);
	}
	
	//NAVIGATION
	public override bool OnTriggerEnterOverride(Collider other, Power owner)
	{
		if(other.tag == GlobalManager.npcsTag)
		{
			PersonStatus person = other.GetComponent<PersonStatus>();
			
			if(person.UnitStatus != PersonStatus.Status.Dead && person.IsAValidTarget)
			{
				if(person.ActivePower != null && person.ActivePower.effectName == "calma")
				{
					person.ActivePower = null;
				}
				
				person.ActivePower = this;
			}
			
		}
		
		return true;
	}

	public override void deliverOnCollisionEffect(Collider other, PersonStatus status)
	{
		
	}
}
