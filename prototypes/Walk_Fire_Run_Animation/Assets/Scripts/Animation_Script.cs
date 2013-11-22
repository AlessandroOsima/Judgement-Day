using UnityEngine;
using System.Collections;

public class Animation_Script : MonoBehaviour
{

	float dt;
	//---------Constants
	public float RunningSpeed =5f;
	public float WalkingSpeed =2f;

	//---------Atributes
	public Vector3 Target; 		//Actual target of the person
	public float Speed;			//Movement speed (NavMesh speed)
	public float FearLevel;	//Determines movement behaviour

	//---------Counters
	private float _burnTimer;
	public float BurnWaitTime=10f;
	public float DeathBurnTime=5f;

	

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
	void StartBurning(){
		_anim.SetInteger("State",3);
		_burnTimer=0;
		SetSpeed(RunningSpeed);
		SetFearLevel(100);
	}
	void Burn(bool burning){
		_anim.SetBool("Burning",burning);
		_emitter.emit=burning;
	}

	void Calm(){
		SetSpeed(WalkingSpeed);
		SetFearLevel(20);
	}

	void Start () {
		//Get Components
		_nav = GetComponent<NavMeshAgent>();
		_anim = GetComponent<Animator>();
		_emitter = GetComponent<ParticleEmitter>();
	}

	// Update is called once per frame
	//List of States:
	// 0 = Dead
	// 1 = Calm
	// 2 = Concerned
	// 3 = Panic
	// 4 = Shocked
	void Update () {
		//--------------Put something to update-------//
		dt=Time.deltaTime;
		if(_anim.GetInteger("State")==0){
			if(_anim.GetBool("Burning")==true){
				_burnTimer += dt;
				if (_burnTimer >= BurnWaitTime){
					Burn (false);
				}
			}
		}
		if(_anim.GetInteger("State")==1){
			if(_anim.GetBool("Burning")==true){
				StartBurning();
			}
		}
		if(_anim.GetInteger("State")==2){
			if(_anim.GetBool("Burning")==true){
				StartBurning();
			}
		}
		if(_anim.GetInteger("State")==3){
			if(_anim.GetBool("Burning")==true){
				_burnTimer += Time.deltaTime;
				if (_burnTimer >= DeathBurnTime){
					SetSpeed(0f);
					_anim.SetInteger("State",0);
				}
			}else{
				_burnTimer=0;
				_anim.SetInteger("State",1);
			}


		}

		if(_anim.GetInteger("State")==4){
			SetSpeed(0f);
			_burnTimer=5;
			Burn (true);
			_anim.SetInteger("State",0);
		}

	}

	//------------Colliding Triggers
	void OnTriggerEnter(Collider other){
		
		if (other.tag=="fire"){
            //Make them Fear with Fire
			Burn(true);
		}

		if (other.tag=="water"){
			//Calm people
			Burn(false);
		}

		if (other.tag=="person"){
			Debug.Log("Hit a person");
			Animator other_anim = other.GetComponent<Animator>();
			if(other_anim.GetBool("Burning")==true){
				Debug.Log("Hit a Burning person");
				Burn(true);
			}
		}
	}

}

