using UnityEngine;
using System.Collections;
using System;

public class AnimationScript : MonoBehaviour
{

	float dt;

    //---------Constants
    public float RunningSpeed = 1f;			//Unit running Speed
    public float MiddleSpeed = 0.4f;		//Unit concerned Speed
    public float WalkingSpeed = 0.2f;		//Unit walking Speed
	public GameObject powerEffect;


    //---------Atributes
    public PersonStatus.Status State;		//Current State
    public int scorePoints = 1;				//How much points gives         ///in personstatus
    public int soulPoints = 3;				//How much souls                /// in personstatus
    public bool Adorer = false;				//If he's an adoring type

    //---------Components
	public Animator _anim;					// Reference to the Animator.
    public ParticleEmitter _fireEmitter;	// Reference to the fire emitter.
	public ParticleEmitter _rageEmitter;	// Reference to the fire emitter.
	private PersonStatus personStatus;

    /*---------Variables
    private NavMeshAgent _nav;      	// Reference to the nav mesh agent.
    private float _movementSpeed;		//Movement speed (NavMesh speed)
    private float _animationSpeed;		//Animation speed (According to movement)*/


    //---------Counters
    public float _burnTimer;			//Timer during burning
    public float BurnWaitTime = 5f;	//Time that it last burning at all
    public float DeathBurnTime = 3f;	//Time that it last burning until dying





    //-----------Set Procedures

    public void SetSpeed(float speed)
    {
        _anim.SetFloat("Speed", speed);
    }


    //Fire:
    public bool isBurning()
    {
        return _anim.GetBool("Burning");
    }

    void Burn(bool burning)
    {
        _anim.SetBool("Burning", burning);
        _fireEmitter.emit = burning;
    }

    void StartBurning()
    {
        Burn(true);
        personStatus.Fear = 100;
		personStatus.UnitStatus = PersonStatus.Status.Panicked;
        _burnTimer = 0;
    }

    //-----------Initialization
    void Start()
    {
        //Get Components
        personStatus = GetComponent<PersonStatus>();
        _anim = GetComponent<Animator>();
        _fireEmitter = GetComponent<ParticleEmitter>();
        _rageEmitter = transform.Find("Rage").GetComponent<ParticleEmitter>();

        if (Adorer) 
			_anim.SetFloat("Adoring", 1f);

		State = personStatus.UnitStatus;
		personStatus.refreshEvents();
		//Register State events
		personStatus.stateTransition += OnStateChanged;
    }

	/*This is ONLY to change the animation based on state change, do NOT add code related to powers or to fear calculation, register another event for that.
	 * 
	 */ 
	void OnStateChanged(PersonStatus.Status prev, PersonStatus.Status current) 
	{
		if (current == PersonStatus.Status.Dead)
		{
			_anim.SetInteger("State",0);
		}
		if (current == PersonStatus.Status.Calm)
		{
			SetSpeed(WalkingSpeed);       
			_anim.SetInteger("State",1);
		}
		if (current == PersonStatus.Status.Concerned)
		{
			SetSpeed(MiddleSpeed);
			_anim.SetInteger("State",2);
		}
		if (current == PersonStatus.Status.Panicked)
		{
			SetSpeed(RunningSpeed);
			_anim.SetInteger("State",3);
		}
		if (current == PersonStatus.Status.Shocked)
		{
			SetSpeed(0f);
			_anim.SetInteger("State",4);
		}
		if (current == PersonStatus.Status.Idle)
		{
			SetSpeed(0f);
			_anim.SetInteger("State",5);
		}
		if (current == PersonStatus.Status.Raged)
		{
			SetSpeed(WalkingSpeed);
			_anim.SetInteger("State",6);
		}
	}

    void Update()
    {
        dt = Time.deltaTime;
        State = personStatus.UnitStatus;

        //If it's Dead
        if (State == PersonStatus.Status.Dead)
        {
            if (isBurning())
            {
                _burnTimer += dt;
                if (_burnTimer >= BurnWaitTime)
                {
                    Burn(false);
                }
            }
        }
        else
        {
            //If it's Alive
            if (State == PersonStatus.Status.Idle)
            {
                if(personStatus.Fear >= 50) 
				{
					personStatus.UnitStatus = PersonStatus.Status.Concerned;
				}

            }
            if (State == PersonStatus.Status.Calm)
            {
                if (personStatus.Fear >= 50) 
				{
					personStatus.UnitStatus = PersonStatus.Status.Concerned;
				}

            }
            if (State == PersonStatus.Status.Concerned)
            {
				if (personStatus.Fear < 50) 
					personStatus.UnitStatus = PersonStatus.Status.Calm;

				if (personStatus.Fear >= 80) 
					personStatus.UnitStatus = PersonStatus.Status.Panicked;

            }

            if (State == PersonStatus.Status.Panicked)
            {
                if (personStatus.Fear < 80)
					personStatus.UnitStatus = PersonStatus.Status.Concerned;

                if (isBurning())
                {
                   _burnTimer += dt;
                   if (_burnTimer >= DeathBurnTime)
                    {
						personStatus.UnitStatus = PersonStatus.Status.Dead;
                    }
                }
            }

            if (State == PersonStatus.Status.Shocked)
            {
                _burnTimer += dt;
                if (_burnTimer >= 1f)
                {
                    Burn(true);
					personStatus.UnitStatus = PersonStatus.Status.Dead;
                }
            }

            if (State == PersonStatus.Status.Raged)
			{
            }

        }
    }

    //------------Colliding Triggers
    void OnTriggerEnter(Collider other) 
    {

		if (personStatus.isAlive() && other != this.collider)
        {
            if(other.tag == "Fire")
            {
               StartBurning();
            }

            if (other.tag == "Water")
            {
                Debug.Log("Hit water");
                if (isBurning())
                {
                    Burn(false);
                    personStatus.Fear = -50;
                }
            }

			if(other.tag == "Finish")
			{
				GlobalManager.globalManager.decrementSouls(10);
				GameObject.Destroy(transform.gameObject);
			}

            if (other.tag == "Person")
            {
				AnimationScript other_anim = other.GetComponent<AnimationScript>();
				PersonStatus otherPerson = other.GetComponent<PersonStatus>();

                    if (other_anim.isBurning())
                    {
                        StartBurning();
                    }
					
					if (otherPerson.UnitStatus == PersonStatus.Status.Raged)
                    {
						Debug.Log("Unit " + this.name + " is killed by " + otherPerson.name);
						personStatus.UnitStatus = PersonStatus.Status.Dead;
                    }
            }
        }
    }

}

