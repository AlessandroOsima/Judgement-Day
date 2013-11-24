using UnityEngine;
using System.Collections;
using System;

public class UnitController : MonoBehaviour {
	float dt;
	//---------Constants
	public float RunningSpeed =5f;
	public float WalkingSpeed =2f;
	//---------Atributes
	public Vector3 Target; 		//Actual target of the person
	public float Speed;			//Movement speed (_navMesh speed)
	public float FearLevel;	//Determines movement behaviour

    public float patrolSpeed = 50f;                          // The _nav mesh agent's speed when patrolling.
    public float chaseSpeed = 5f;                           // The _nav mesh agent's speed when chasing.
    public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
    public float patrolWaitTime = 2f;                       // The amount of time to wait when the patrol way point is reached.
    public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.

    //private LastPlayerSighting lastPlayerSighting;
    //private PersonSight personSight;                          // Reference to the EnemySight script.
    private float chaseTimer;                               // A timer for the chaseWaitTime.
    private float patrolTimer;                              // A timer for the patrolWaitTime.
    private int wayPointIndex;  

		
	//---------Components
	private Animator _anim;
	private NavMeshAgent _nav;  // Reference to the _nav mesh agent.


	//-----------Set Procedures
	void SetFearLevel (int Fear)
	{
		FearLevel = Fear/100f;
		_anim.SetFloat("Speed",FearLevel);
	}
	
	void SetSpeed (float speed)
	{
		Speed = speed;
		_nav.speed=Speed;
		if(Speed>0)
			_nav.acceleration=Speed;
	}

	//---------Use this for initialization
	void Start () {
        //Debug.Log("Start");
		//Get Components
		_nav = GetComponent<NavMeshAgent>();
		_anim = GetComponent<Animator>();
		SetNewDestination();
		//_nav.destination = patrolWayPoints[wayPointIndex].position;
		SetSpeed(WalkingSpeed);
		SetFearLevel(20);
        
       
		//Set Atributes
		//Target = GameObject.Find("_target1").transform.position;
		
		//Set destination and speed
		//_nav.destination = Target;
		//SetSpeed(3f);
		//SetFearLevel(0.1f);
        //Debug.Log(patrolWayPoints[0].position);
        //Debug.Log(patrolWayPoints[0].transform.position);
	}
	
	
	void Update () {
		//--------------Put something to update-------//
        Patrolling();
        
	}
	
	

	
	void SetNewDestination(){
		System.Random rnd = new System.Random();
		int Random_num;
		Random_num = rnd.Next(0,patrolWayPoints.Length);

		while (patrolWayPoints[Random_num] == null || Random_num == wayPointIndex)
		{
			Random_num = rnd.Next(0,patrolWayPoints.Length);

			if(patrolWayPoints[Random_num] == null)
			{
				throw new NullReferenceException("Waypoints " + Random_num + " in patrolWaypoints is null, you must define it in the editor");
			}
		}

		wayPointIndex = Random_num;
		_nav.destination = patrolWayPoints[wayPointIndex].position;
		Target = _nav.destination;
		//Debug.Log("Going to Target: " + (wayPointIndex+1));
	}
	
	

    void Patrolling()
    {
		//Debug.Log("Still " + _nav.remainingDistance + " to " + _nav.stoppingDistance);
        // Set an appropriate speed for the _navMeshAgent.
       // _nav.speed = patrolSpeed;

		if(_anim.GetInteger("State") > 0 && _anim.GetInteger("State") < 4)
		{		
	        // If near the next waypoint or there is no destination...
			if (_nav.remainingDistance == _nav.stoppingDistance)
			{
				//Debug.Log("Arrived at Target: "+ (wayPointIndex + 1));
			}
			if (_nav.remainingDistance < _nav.stoppingDistance)
	        {
				if(_anim.GetBool("Burning")==false){
					// ... increment the timer.
					dt = Time.deltaTime;
		            patrolTimer += dt;
					SetSpeed(0f);
					SetFearLevel(0);
		            // If the timer exceeds the wait time...
		            if (patrolTimer >= patrolWaitTime)
		            {
		                // ... increment the wayPointIndex.-----------Scelta dei waypoint!!!!
		                /*if (wayPointIndex == patrolWayPoints.Length - 1)
		                    wayPointIndex = 0;
		                else
		                    wayPointIndex++;*/
						 // Set the destination to the patrolWayPoint.
		                // Reset the timer.
		                patrolTimer = 0;
						SetNewDestination();
						SetSpeed(WalkingSpeed);
						SetFearLevel(20);
		            }
				}else{
					SetNewDestination();
				}
	        }else{
				patrolTimer = 0;
				if(_anim.GetInteger("State")==3){
					// If not near a destination, reset the timer.
					SetSpeed(RunningSpeed);
					SetFearLevel(100);
				}
				if(_anim.GetInteger("State")==1){
		            // If not near a destination, reset the timer.
					SetSpeed(WalkingSpeed);
					SetFearLevel(20);
				}
			}
		}
        
    }
	
}
