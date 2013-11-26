using UnityEngine;
using System.Collections;
using System;

public class UnitNavigationController : MonoBehaviour
{
	float dt;
	System.Random rnd;

	public enum PatrolType
	{
		Patrol,
		Fixed,
		Idle,
		Panic
	}

	//---------Constants
	public float RunningSpeed = 5f;		//Unit running Speed
	public float MiddleSpeed  = 3.5f;	//Unit concerned Speed
	public float WalkingSpeed = 2f;		//Unit walking Speed
	public float patrolWaitTime = 3f;	// The amount of time to wait when the patrol way point is reached.

	//---------Atributes
	public PatrolType Type = PatrolType.Idle;	//Type of Unit
	public AnimationScript.AnimState State;						//Current State
	public bool Randomness = true;
	public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.
	public Transform[] panicWayPoints;                     	// An array of transforms for the running route.
	//public Vector3 Target; 			//Actual target of the person
	//public Transform[] otherWayPoints;					// An array of transforms for other route.
    //public float patrolSpeed = 50f;                       // The _nav mesh agent's speed when patrolling.
    //public float chaseSpeed = 5f;                         // The _nav mesh agent's speed when chasing.
    //public float chaseWaitTime = 5f;                      // The amount of time to wait when the last sighting is reached.
    //private LastPlayerSighting lastPlayerSighting;
    //private PersonSight personSight;                      // Reference to the EnemySight script.
    //private float chaseTimer;                               // A timer for the chaseWaitTime.

	//---------Counters
    private float patrolTimer;			// A timer for the patrolWaitTime.
    private int wayPointIndex;			// Index of the current Waypoint
	private bool Stand=false;

		
	//---------Components
	private NavMeshAgent _nav;  // Reference to the _nav mesh agent.
	AnimationScript _animator;	// Reference to the Animator_Script Class


	//-----------Set Procedures
	void SetSpeed (float speed){
		_nav.speed=speed;
		if(speed>0)
			_nav.acceleration=speed;
	}

	void SetNewDestination(PatrolType Type_Patrol){
		int Random_num;
		Transform[] WayPoints;

		if(Type_Patrol==PatrolType.Patrol || Type_Patrol==PatrolType.Fixed){
			WayPoints=patrolWayPoints;
		}else
		if(Type_Patrol==PatrolType.Panic || Type_Patrol==PatrolType.Idle){
			WayPoints=panicWayPoints;
		}else WayPoints=null;

		if(Type_Patrol==PatrolType.Fixed){
			wayPointIndex++;
			if(wayPointIndex>=WayPoints.Length) wayPointIndex=0;
		}else{
			Random_num = rnd.Next(0,WayPoints.Length);
			while (Random_num == wayPointIndex){
				Random_num = rnd.Next(0,WayPoints.Length);
			}
			wayPointIndex = Random_num;
		}
		_nav.destination = WayPoints[wayPointIndex].position;
		Debug.Log(transform.name+" going to Target: " + (WayPoints[wayPointIndex].name));
	}
	
	
	
	void Patrolling(){
		// If near the next waypoint or there is no destination...
		if(_nav.destination==null){
			SetNewDestination(Type);
		}
		if (_nav.remainingDistance == _nav.stoppingDistance){
			Debug.Log("Arrived to Target: "+ (patrolWayPoints[wayPointIndex].name));
		}
		if (_nav.remainingDistance < _nav.stoppingDistance){
			if(Type==PatrolType.Idle){
				SetNewDestination(Type);
				//SetSpeed(MiddleSpeed);
			}else
			if(State==AnimationScript.AnimState.Calm)
				Stand=true;
		}
		if(Stand){
			patrolTimer += dt;
			if (Randomness){
				patrolTimer -= UnityEngine.Random.value/100f;
				patrolTimer += UnityEngine.Random.value/100f;
			}
			_animator.ChangeState(AnimationScript.AnimState.Idle);
			if (patrolTimer >= patrolWaitTime){
				patrolTimer = 0;
				SetNewDestination(Type);
				Stand=false;
				_animator.ChangeState(AnimationScript.AnimState.Calm);
			}
		}else{
			if (Randomness){
				patrolTimer += dt;
				if (patrolTimer >= patrolWaitTime){
					if(UnityEngine.Random.value>0.99){
						Stand=true;
						patrolTimer = 0;
					}
					if(UnityEngine.Random.value>0.999){
						SetNewDestination(PatrolType.Patrol);
						patrolTimer = 0;
					}
				}
			}else{
				patrolTimer = 0;
			}
		}
	}
	
	void Panicking(){
		// If near the next waypoint or there is no destination...
		if (_nav.remainingDistance == _nav.stoppingDistance){
			Debug.Log("Arrived to Target: "+ (panicWayPoints[wayPointIndex].name));
		}
		if (_nav.remainingDistance < _nav.stoppingDistance){
			SetNewDestination(PatrolType.Panic);
		}else{
			if (Randomness){
				if(UnityEngine.Random.value>0.999)	SetNewDestination(PatrolType.Panic);
			}
		}
	}

	void SetTarget(){
		GameObject[] Persons = GameObject.FindGameObjectsWithTag(GlobalManager.npcsTag);
		float closestDist = Mathf.Infinity; 
		int closest = -1;
		for (int i=0;i<Persons.Length;i++) { 
			if(Persons[i].GetComponent<AnimationScript>().isAlive()){
				float dist = (transform.position - Persons[i].transform.position).sqrMagnitude; 
				
				if (dist!=0 && dist < closestDist) { 
					closestDist = dist; 
					closest = i; 
				}
			}
		}
		if(closest!=-1)
			_nav.destination = Persons[closest].transform.position;
	}



//---------Use this for initialization
	void Start () {
		//Get Components
		_nav = GetComponent<NavMeshAgent>();
		_animator = GetComponent<AnimationScript>();
		rnd = new System.Random();
		State=_animator.ReturnState();
		//Debug.Log(transform.name+": "+State);
		if(State==AnimationScript.AnimState.Calm){
			if(Type==PatrolType.Patrol){
				SetNewDestination(Type);
				SetSpeed(WalkingSpeed);
			}
			if(Type==PatrolType.Fixed){
				Randomness=false;
				SetNewDestination(Type);
				SetSpeed(WalkingSpeed);
			}
		}
		if(State==AnimationScript.AnimState.Concerned){
			//if(Type=="Patrol"){
				SetNewDestination(PatrolType.Patrol);
				SetSpeed(MiddleSpeed);
			//}
		}
		if(State==AnimationScript.AnimState.Panic){
			SetNewDestination(PatrolType.Panic);
			SetSpeed(RunningSpeed);
		}
	}
	
	
	// Update is called once per frame
	void Update () {
		dt=Time.deltaTime;
		//--------------Put something to update-------//
		if(_animator.isAlive()){
			State=_animator.ReturnState();
			if(State==AnimationScript.AnimState.Shocked){
				SetSpeed(0f);
			}
			if(State==AnimationScript.AnimState.Idle){
				if(Type==PatrolType.Patrol){
					Patrolling();
				}
				if(Type==PatrolType.Fixed){
					Patrolling();
				}
			}
			if(State==AnimationScript.AnimState.Calm){
				if(Type==PatrolType.Patrol){
					SetSpeed(WalkingSpeed);
					Patrolling();
				}
				if(Type==PatrolType.Fixed){
					SetSpeed(WalkingSpeed);
					Patrolling();
				}
			}
			if(State==AnimationScript.AnimState.Concerned){
				//SetNewDestination(Type);
				Debug.Log (_nav.destination);
				SetSpeed(MiddleSpeed);
				Patrolling();
			}


			if(State==AnimationScript.AnimState.Panic){
				SetSpeed(RunningSpeed);
				Panicking();
			}

			if(State==AnimationScript.AnimState.Raged){
				SetTarget();
				SetSpeed(MiddleSpeed);
			}

		}else{
			SetSpeed(0f);
		}

	}
	
	

	

}
