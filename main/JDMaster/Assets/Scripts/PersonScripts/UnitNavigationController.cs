using UnityEngine;
using System.Collections;
using System;

public class UnitNavigationController : MonoBehaviour
{
	public enum PatrolType
	{
		Patrol,
		Fixed,
		Idle,
		Panic
	}

    float dt;
    System.Random rnd;
    //---------Constants
    public float RunningSpeed = 5f;		//Unit running Speed
	public float MiddleSpeed = 3.5f;	//Unit concerned Speed
	public float JoggingSpeed = 3f;	
	public float WalkingSpeed = 2f;		//Unit walking Speed
	public float patrolWaitTime = 3f;	// The amount of time to wait when the patrol way point is reached.
	public PersonStatus target = null;

    //---------Atributes
    public PatrolType Type = PatrolType.Idle;	//Type of Unit
    public PersonStatus.Status State;						//Current State
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
    private bool Stand = false;


    //---------Components
    public NavMeshAgent _nav;  // Reference to the _nav mesh agent.
    private PersonStatus ps;	        // Reference to the PersonStatus Class


    //-----------Set Procedures
    public void SetSpeed(float speed)
    {
        _nav.speed = speed;
	
        if (speed > 0)
           	 _nav.acceleration = speed;
    }

	public void Stop()
	{
		SetSpeed(0);
		_nav.acceleration = 10;
	}


	public void SetNavDestination(Vector3 position)
	{
		_nav.destination = position;
	}

	public bool isAtDestination()
	{
		return _nav.remainingDistance <= _nav.stoppingDistance;
	}

    public void SetNewPatrolDestination(PatrolType Type_Patrol)
    {
        int Random_num;
        Transform[] WayPoints;
		target = null; 

        if (Type_Patrol == PatrolType.Patrol || Type_Patrol == PatrolType.Fixed)
        {
            WayPoints = patrolWayPoints;
        }
        else if ((Type_Patrol == PatrolType.Panic || Type_Patrol == PatrolType.Idle) && panicWayPoints.Length > 0)
        {
            WayPoints = panicWayPoints;
        }
        else 
		{
			if(patrolWayPoints.Length > 0)
				WayPoints = patrolWayPoints;
			else
			{
				_nav.destination = this.transform.position;
				return;
			}
			//Debug.Log("No waypoints array is selected, something VERY WRONG is going on");
		}


        if (Type_Patrol == PatrolType.Fixed)
        {
            wayPointIndex++;

            if (wayPointIndex >= WayPoints.Length) 
				wayPointIndex = 0;
        }
        else
        {
            Random_num = rnd.Next(0, WayPoints.Length);

            while (Random_num == wayPointIndex && WayPoints.Length > 0)
            {
                Random_num = rnd.Next(0, WayPoints.Length);
            }

            wayPointIndex = Random_num;
        }

		if(WayPoints.Length == 0)
        	_nav.destination = this.transform.position;
		else
			_nav.destination = WayPoints[wayPointIndex].position;

        //Debug.Log(transform.name + " going to Target: " + (WayPoints[wayPointIndex].name));
    }



    void Patrolling()
    {
        // If near the next waypoint or there is no destination...
        if (isAtDestination())
        {
			SetNewPatrolDestination(Type);
		}

        if (_nav.remainingDistance <= _nav.stoppingDistance)
        {
			Stop();

            if (Type == PatrolType.Idle)
            {
                SetNewPatrolDestination(Type);
            }
            else if (State == PersonStatus.Status.Calm)
			{
				Stand = true;
				ps.UnitStatus = PersonStatus.Status.Idle;
			}
        }

        if (Stand)
        {
            patrolTimer += dt;
            if (Randomness)
            {
                patrolTimer -= UnityEngine.Random.value / 100f;
                patrolTimer += UnityEngine.Random.value / 100f;
            }
             //_animator.ChangeState(PersonStatus.Status.Idle);

            if (patrolTimer >= patrolWaitTime)
            {
                patrolTimer = 0;
				SetNewPatrolDestination(Type);
				Stand = false;
				ps.UnitStatus = PersonStatus.Status.Calm;
		
			}
		}
        else
        {
            if (Randomness)
            {
                patrolTimer += dt;
                if (patrolTimer >= patrolWaitTime)
                {
                    if (UnityEngine.Random.value > 0.99)
                    {
                        Stand = true;
                        patrolTimer = 0;
                    }
                    if (UnityEngine.Random.value > 0.999)
                    {
                        SetNewPatrolDestination(PatrolType.Patrol);
                        patrolTimer = 0;
                    }
                }
            }
            else
            {
                patrolTimer = 0;
            }
        }
    }

    public void Panicking()
    {
        // If near the next waypoint or there is no destination...
        if (_nav.remainingDistance < _nav.stoppingDistance)
        {
            SetNewPatrolDestination(PatrolType.Panic);
        }
        else
        {
            if (Randomness)
            {
                if (UnityEngine.Random.value > 0.999) SetNewPatrolDestination(PatrolType.Panic);
            }
        }
    }
	
    //---------Use this for initialization
    void Start()
    {
        //Get Components
        _nav = GetComponent<NavMeshAgent>();
        ps = GetComponent<PersonStatus>();
        rnd = new System.Random();
        State = ps.UnitStatus;
		ps.refreshEvents();

        if (State == PersonStatus.Status.Calm)
        {
            if (Type == PatrolType.Patrol)
            {
                SetNewPatrolDestination(Type);
                SetSpeed(WalkingSpeed);
            }
            if (Type == PatrolType.Fixed)
            {
                Randomness = false;
                SetNewPatrolDestination(Type);
                SetSpeed(WalkingSpeed);
            }
        }

        if (State == PersonStatus.Status.Concerned)
        {
            //if(Type=="Patrol"){
            SetNewPatrolDestination(PatrolType.Patrol);
            SetSpeed(MiddleSpeed);
            //}
        }

        if (State == PersonStatus.Status.Panicked)
        {
            SetNewPatrolDestination(PatrolType.Panic);
            SetSpeed(RunningSpeed);
        }
    }


    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        //--------------Put something to update-------//
        if (ps.isAlive())
        {
            State = ps.UnitStatus;

			if(ps.ActivePower != null)
			{
				ps.ActivePower.runNavigatorUpdate(this);
				return;
			}

            if (State == PersonStatus.Status.Shocked)
            {
                SetSpeed(0f);
            }

            if (State == PersonStatus.Status.Idle)
            {
                if (Type == PatrolType.Patrol)
                {
                    Patrolling();
                } 
                if (Type == PatrolType.Fixed)
                {
                    Patrolling();
                }
            }
            if (State == PersonStatus.Status.Calm)
            {
                if (Type == PatrolType.Patrol)
                {
                    SetSpeed(WalkingSpeed);
                    Patrolling();
                }
                if (Type == PatrolType.Fixed)
                {
                    SetSpeed(WalkingSpeed);
                    Patrolling();
                }
            }
            if (State == PersonStatus.Status.Concerned)
            {
                SetSpeed(MiddleSpeed);
                Patrolling();
            }


            if (State == PersonStatus.Status.Panicked)
            {
                SetSpeed(RunningSpeed);
                Panicking();
            }
        }
        else
        {
            SetSpeed(0f);
        }

    }
}
