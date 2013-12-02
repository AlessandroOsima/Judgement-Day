using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour
{

	float dt;

	public enum AnimState
	{
		Dead,
		Calm,
		Concerned,
		Panic,
		Shocked,
		Idle,
		Raged
	}

	//---------Constants
	public float RunningSpeed = 1f;			//Unit running Speed
	public float MiddleSpeed  = 0.4f;		//Unit concerned Speed
	public float WalkingSpeed = 0.2f;		//Unit walking Speed


	//---------Atributes
	public AnimState State ;				//Current State
	public int FearLevel;					//Determines movement behaviour ///in personstatus
    public int scorePoints = 1;				//How much points gives         ///in personstatus
    public int soulPoints = 3;				//How much souls                /// in personstatus
	public bool Adorer=false;				//If he's an adoring type

	//---------Components
	private Animator _anim;					// Reference to the Animator.
	private ParticleEmitter _fireEmitter;	// Reference to the fire emitter.
	private ParticleEmitter _rageEmitter;	// Reference to the fire emitter.

	/*---------Variables
	private NavMeshAgent _nav;      	// Reference to the nav mesh agent.
	private float _movementSpeed;		//Movement speed (NavMesh speed)
	private float _animationSpeed;		//Animation speed (According to movement)*/


	//---------Counters
	public float _burnTimer;			//Timer during burning
	public float BurnWaitTime	= 5f;	//Time that it last burning at all
	public float DeathBurnTime	= 3f;	//Time that it last burning until dying

	



	//-----------Set Procedures
	void SetFearLevel (int Ammount){
		FearLevel += Ammount;
		if(FearLevel>100) FearLevel=100;
		if(FearLevel<0) FearLevel=0;
	}

	void SetSpeed (float speed){
		_anim.SetFloat("Speed",speed);
	}

	//-----------Animation Procedures
	public bool isAlive(){
		return ReturnState()!=AnimState.Dead;
	}
	//Fire:
	public bool isBurning(){
		return _anim.GetBool("Burning");
	}
	void Burn(bool burning){
		_anim.SetBool("Burning",burning);
		_fireEmitter.emit=burning;
	}
	void StartBurning(){
		Burn(true);
		SetFearLevel(100);
		ChangeState(AnimState.Panic);
		_burnTimer=0;
	}
		


	
	//-----List of States:
	//"Dead" 		return 0;
	//"Calm"		return 1;
	//"Concerned" 	return 2;
	//"Panic"		return 3;
	//"Shocked"		return 4;
	//"Idle"		return 5;
	
	
	public AnimState ReturnState(){
		if(_anim.GetInteger("State")==0)		return AnimState.Dead;
		if(_anim.GetInteger("State")==1) 		return AnimState.Calm;
		if(_anim.GetInteger("State")==2) 		return AnimState.Concerned;
		if(_anim.GetInteger("State")==3)		return AnimState.Panic;
		if(_anim.GetInteger("State")==4) 		return AnimState.Shocked;
		if(_anim.GetInteger("State")==5)		return AnimState.Idle;
		if(_anim.GetInteger("State")==6)		return AnimState.Raged;
		else return AnimState.Idle;
	}
	
	public void ChangeState(AnimState action){
		if(action==AnimState.Dead){
			_anim.SetInteger("State",0);
			GlobalManager.globalManager.decrementPopulation(1);
			GlobalManager.globalManager.incrementScore(scorePoints);
			GlobalManager.globalManager.incrementSouls(soulPoints);
		}
		if(action==AnimState.Calm){
			SetSpeed(WalkingSpeed);
			_anim.SetInteger("State",1);
		}
		if(action==AnimState.Concerned){
			SetSpeed(MiddleSpeed);
			_anim.SetInteger("State",2);
		}
		if(action==AnimState.Panic)	{
			SetSpeed(RunningSpeed);
			_anim.SetInteger("State",3);
		}
		if(action==AnimState.Shocked){
			//SetSpeed(0f);
			_anim.SetInteger("State",4);
		}
		if(action==AnimState.Idle){
			SetSpeed(0f);
			_anim.SetInteger("State",5);
		}
		if(action==AnimState.Raged){
			SetSpeed(WalkingSpeed);
			_anim.SetInteger("State",6);
		}
		
	}



	//-----------Initialization
	void Start () {
		//Get Components
		_anim = GetComponent<Animator>();
		_rageEmitter = GetComponentInChildren<ParticleEmitter>();
		_fireEmitter = GetComponent<ParticleEmitter>();
		if(Adorer) _anim.SetFloat("Adoring",1f);
		ChangeState(State);
	}

	//------Update is called once per frame

	void Update () {
		dt=Time.deltaTime;
		State=ReturnState();
		//If it's Dead
		if(State==AnimState.Dead){
			if(isBurning()){
				_burnTimer += dt;
				if (_burnTimer >= BurnWaitTime){
					Burn (false);
				}
			}
		}else{
		//If it's Alive
			if(State==AnimState.Idle){
				if(FearLevel>=50) ChangeState(AnimState.Concerned);
				
				/*if(isBurning()){
					StartBurning();
				}else{
					//SetSpeed(0f);
					//SetFearLevel(20);
				}*/
			}
			if(State==AnimState.Calm){
				if(FearLevel>=50) ChangeState(AnimState.Concerned);
				//ChangeState(AnimState.Calm);
				/*if(isBurning()){
					StartBurning();
				}else{
					//SetSpeed(0f);
					//SetFearLevel(20);
				}*/
			}
			if(State==AnimState.Concerned){
				if(FearLevel<50) ChangeState(AnimState.Calm);
				if(FearLevel>=80) ChangeState(AnimState.Panic);
				//ChangeState(AnimState.Concerned);
				/*if(isBurning()){
					StartBurning();
				}else{
					//SetSpeed(0f);
					//SetFearLevel(20);
				}*/
			}
			if(State==AnimState.Panic){
				if(FearLevel<80) ChangeState(AnimState.Concerned);
				if(isBurning()){
					_burnTimer += dt;
					if (_burnTimer >= DeathBurnTime){
						ChangeState(AnimState.Dead);
					}
				}else{
					//_burnTimer=0;
					//ChangeState(AnimState.Dead);
				}
			}

			if(State==AnimState.Shocked){
				//SetSpeed(0f);
				_burnTimer += dt;
				if (_burnTimer >= 0.5f){
					Burn (true);
					ChangeState(AnimState.Dead);
				}
			}

			if(State==AnimState.Raged){
				//SetSpeed(0f);
				_burnTimer += dt;
				if (_burnTimer >= 10f ){
					ChangeState(AnimState.Concerned);
					SetFearLevel(50);
					_rageEmitter.emit=false;	
				}
			}

		}
	}

	//------------Colliding Triggers
	void OnTriggerEnter(Collider other){
		if (isAlive()){
			if (other.tag=="Fire"){
				StartBurning(); 	
			}

			if (other.tag=="Rage"){
				if(!isBurning()){
					_burnTimer = 0;
					ChangeState(AnimState.Raged);
					_rageEmitter.emit=true;	
				}
			}

			if (other.tag=="Water"){
				Debug.Log("Hit water");
				if(isBurning()){
					Burn(false);
					SetFearLevel(-50);
				}
			}

			if (other.tag=="Lightning"){
				ChangeState(AnimState.Shocked);
			}

			if (other.tag=="Power"){
				State=ReturnState();
				if(State!=AnimState.Dead && State!=AnimState.Shocked){
				//if(FearLevel<50) ChangeState(AnimState.Calm);
				//if(FearLevel>=50) ChangeState(AnimState.Concerned);
				//if(FearLevel>=80) ChangeState(AnimState.Panic);
					PowerScript other_power = other.GetComponent<PowerScript>();
					SetFearLevel(other_power.Fear);
				}
			}
			
			if (other.tag=="Person"){
				if(!isBurning()){
					AnimationScript other_anim = other.GetComponent<AnimationScript>();
					if(other_anim.isBurning()){
						Debug.Log("Hit a Burning person");
						StartBurning();
					}
					if(other_anim.ReturnState()==AnimState.Raged){
						ChangeState(AnimState.Dead);
					}
					/*if(State==AnimState.Raged){
						_rageEmitter.emit=false;	
						ChangeState(AnimState.Concerned);
						SetFearLevel(50);
					}*/

				}
			}
		}
	}

}

