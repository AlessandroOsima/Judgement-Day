using UnityEngine;
using System.Collections;

public class Animation_Script : MonoBehaviour
{

	//---------Atributes
	public Vector3 Target; 		//Actual target of the person
	public float Speed;			//Movement speed (NavMesh speed)
	public float FearLevel;	//Determines movement behaviour

	//---------Components
	
	private Animator _anim;
	private NavMeshAgent _nav;                // Reference to the nav mesh agent.
	private ParticleEmitter _emitter;

	//-----------Set Procedures
	void SetFearLevel (int Fear){
		FearLevel = Fear/100f;
		_anim.SetFloat("Speed",FearLevel);
	}
	
	void SetSpeed (float speed){
		Speed = speed;
		_nav.speed=Speed;
		if(Speed>0)
			_nav.acceleration=Speed;
	}

	//-----------Fear Procedures

	void Burn(){
		SetSpeed(5f);
		SetFearLevel(100);
		_anim.SetBool("Burning",true);
		_emitter.emit=true;
	}

	void Calm(){
		SetSpeed(3f);
		SetFearLevel(20);
		_anim.SetBool("Burning",false);
		_emitter.emit=false;
	}


	void Start () {
		//Get Components
		_nav = GetComponent<NavMeshAgent>();
		_anim = GetComponent<Animator>();
		_emitter = GetComponent<ParticleEmitter>();
	}

	//------------Colliding Triggers
	void OnTriggerEnter(Collider other){
		
		if (other.tag=="fire"){
            //Make them Fear with Fire
			Burn();
		}

		if (other.tag=="water"){
			//Calm people
			Calm();
		}

		if (other.tag=="person"){
			Debug.Log("Hit a person");
			Animator other_anim = GetComponent<Animator>();
			if(other_anim.GetBool("Burning")==true){
				Debug.Log("Hit a Burning person");
				Burn();
			}
		}
	}

}

