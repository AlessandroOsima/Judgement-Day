using UnityEngine;
using System.Collections;

public class ObjectBurn : ValidTarget {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartBurning(){
		ParticleEmitter InnerFlameEmitter = transform.GetChild(0).GetComponent<ParticleEmitter>();
		ParticleEmitter OuterFlameEmitter = transform.GetChild(1).GetComponent<ParticleEmitter>();
		ParticleEmitter SmokeEmitter = transform.GetChild(2).GetComponent<ParticleEmitter>();
		InnerFlameEmitter.emit=true;
		OuterFlameEmitter.emit=true;
		SmokeEmitter.emit=true;
		transform.tag="fire";
	}

	//------------Colliding Triggers
	void OnTriggerEnter(Collider other){
		if (other.tag=="fire" || other.tag=="lightning"){
				StartBurning(); 	
		}
		if (other.tag=="person"){
			AnimationScript other_anim = other.GetComponent<AnimationScript>();
			if(other_anim.isBurning()){
				StartBurning();
			}
		}
	}
}
