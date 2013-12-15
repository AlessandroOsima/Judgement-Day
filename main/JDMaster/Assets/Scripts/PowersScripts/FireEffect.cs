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
			started = true;
			status.UnitStatus = PersonStatus.Status.Panicked;
			animator.powerEffect = (GameObject) Object.Instantiate(owner.particleEffect, new Vector3(status.transform.position.x,status.transform.position.y + 0.9f,status.transform.position.z),Quaternion.identity);
			animator.powerEffect.transform.parent = animator.transform;
			status.Fear = owner.Fear;
		}
		else if(status.UnitStatus != PersonStatus.Status.Raged && status.UnitStatus != PersonStatus.Status.Dead)
		{
			timer += Time.deltaTime;
			if (timer >= 10f)
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

	public override bool OnTriggerEnterOverride(Collider other, Power owner)
	{
		return false;
	}
}
