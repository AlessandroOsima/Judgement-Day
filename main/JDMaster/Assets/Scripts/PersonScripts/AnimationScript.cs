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


    //---------Atributes
    public PersonStatus.Status State;		//Current State
    public int FearLevel;					//Determines movement behaviour ///in personstatus
    public int scorePoints = 1;				//How much points gives         ///in personstatus
    public int soulPoints = 3;				//How much souls                /// in personstatus
    public bool Adorer = false;				//If he's an adoring type

    //---------Components
    private Animator _anim;					// Reference to the Animator.
    private ParticleEmitter _fireEmitter;	// Reference to the fire emitter.
    private ParticleEmitter _rageEmitter;	// Reference to the fire emitter.
    private PersonStatus ps;

    /*---------Variables
    private NavMeshAgent _nav;      	// Reference to the nav mesh agent.
    private float _movementSpeed;		//Movement speed (NavMesh speed)
    private float _animationSpeed;		//Animation speed (According to movement)*/


    //---------Counters
    public float _burnTimer;			//Timer during burning
    public float BurnWaitTime = 5f;	//Time that it last burning at all
    public float DeathBurnTime = 3f;	//Time that it last burning until dying





    //-----------Set Procedures
    void SetFearLevel(int Ammount)
    {
        FearLevel += Ammount;
        if (FearLevel > 100) FearLevel = 100;
        if (FearLevel < 0) FearLevel = 0;
    }

    void SetSpeed(float speed)
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
        SetFearLevel(100);
        ChangeState(PersonStatus.Status.Panicked); 
        _burnTimer = 0;
    }




    //-----List of States:
    //"Dead" 		return 0;
    //"Calm"		return 1;
    //"Concerned" 	return 2;
    //"Panic"		return 3;
    //"Shocked"		return 4;
    //"Idle"		return 5;


    public PersonStatus.Status ReturnState()
    {


        if (ps.UnitStatus == PersonStatus.Status.Dead)
            return PersonStatus.Status.Dead;
        else if (ps.UnitStatus == PersonStatus.Status.Calm)
            return PersonStatus.Status.Calm;
        else if (ps.UnitStatus == PersonStatus.Status.Concerned)
            return PersonStatus.Status.Concerned;
        else if (ps.UnitStatus == PersonStatus.Status.Panicked)
            return PersonStatus.Status.Panicked;
        else if (ps.UnitStatus == PersonStatus.Status.Shocked)
            return PersonStatus.Status.Shocked;
        else if (ps.UnitStatus == PersonStatus.Status.Idle)
            return PersonStatus.Status.Idle;
        else if (ps.UnitStatus == PersonStatus.Status.Raged)
            return PersonStatus.Status.Raged;
        else return PersonStatus.Status.Idle;

    }

    public void ChangeState(PersonStatus.Status action)
    {

        if (action == PersonStatus.Status.Dead)
        { 
			ps.UnitStatus = action;
            GlobalManager.globalManager.decrementPopulation(1);
            GlobalManager.globalManager.incrementScore(scorePoints);
            GlobalManager.globalManager.incrementSouls(soulPoints);
			_anim.SetInteger("State",0);
        }
        if (action == PersonStatus.Status.Calm)
        {
            SetSpeed(WalkingSpeed);
			ps.UnitStatus = action;            
			_anim.SetInteger("State",1);
        }
        if (action == PersonStatus.Status.Concerned)
        {
            SetSpeed(MiddleSpeed);
			ps.UnitStatus = action;
            _anim.SetInteger("State",2);
        }
        if (action == PersonStatus.Status.Panicked)
        {
            SetSpeed(RunningSpeed);
			ps.UnitStatus = action;
            _anim.SetInteger("State",3);
        }
        if (action == PersonStatus.Status.Shocked)
        {
            //SetSpeed(0f);
			ps.UnitStatus = action;
            _anim.SetInteger("State",4);
        }
        if (action == PersonStatus.Status.Idle)
        {
            SetSpeed(0f);
			ps.UnitStatus = action;
            _anim.SetInteger("State",5);
        }
        if (action == PersonStatus.Status.Raged)
        {
            SetSpeed(WalkingSpeed);
			ps.UnitStatus = action;
            _anim.SetInteger("State",6);
        }

    }



    //-----------Initialization
    void Start()
    {
        //ps = new PersonStatus();
        //Get Components
        ps = GetComponent<PersonStatus>();
        _anim = GetComponent<Animator>();
        _fireEmitter = GetComponent<ParticleEmitter>();
        _rageEmitter = transform.Find("Rage").GetComponent<ParticleEmitter>();
        if (Adorer) _anim.SetFloat("Adoring", 1f);
		State = ps.UnitStatus;
        ChangeState(State);
    }

    //------Update is called once per frame

    void Update()
    {
        dt = Time.deltaTime;
        State = ps.UnitStatus;
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
                if (FearLevel >= 50) 
				{
					ChangeState(PersonStatus.Status.Concerned);
				}
				else
				{
					_anim.SetInteger("State",5);
					SetSpeed(0f);
				}

                /*if(isBurning()){
                    StartBurning();
                }else{
                    //SetSpeed(0f);
                    //SetFearLevel(20);
                }*/
            }
            if (State == PersonStatus.Status.Calm)
            {
                if (FearLevel >= 50) 
				{
					ChangeState(PersonStatus.Status.Concerned);
				}
				else
				{
					SetSpeed(WalkingSpeed);
					_anim.SetInteger("State",1);
				}
                //ChangeState(AnimState.Calm);
                /*if(isBurning()){
                    StartBurning();
                }else{
                    //SetSpeed(0f);
                    //SetFearLevel(20);
                }*/
            }
            if (State == PersonStatus.Status.Concerned)
            {
                if (FearLevel < 50) ChangeState(PersonStatus.Status.Calm);
                if (FearLevel >= 80) ChangeState(PersonStatus.Status.Panicked);
                //ChangeState(AnimState.Concerned);
                /*if(isBurning()){
                    StartBurning();
                }else{
                    //SetSpeed(0f);
                    //SetFearLevel(20);
                }*/
            }
            if (State == PersonStatus.Status.Panicked)
            {
                if (FearLevel < 80) ChangeState(PersonStatus.Status.Concerned);
                if (isBurning())
                {
                    _burnTimer += dt;
                    if (_burnTimer >= DeathBurnTime)
                    {
                        ChangeState(PersonStatus.Status.Dead);
                    }
                }
                else
                {
                    //_burnTimer=0;
                    //ChangeState(AnimState.Dead);
                }
            }

            if (State == PersonStatus.Status.Shocked)
            {
                //SetSpeed(0f);
                _burnTimer += dt;
                if (_burnTimer >= 0.5f)
                {
                    Burn(true);
                    ChangeState(PersonStatus.Status.Dead);
                }
            }

            if (State == PersonStatus.Status.Raged)
            {
                //SetSpeed(0f);
                _burnTimer += dt;
                if (_burnTimer >= 10f)
                {
                    ChangeState(PersonStatus.Status.Concerned);
                    SetFearLevel(50);
                    _rageEmitter.emit = false;
                }
            }

        }
    }

    //------------Colliding Triggers
    void OnTriggerEnter(Collider other)
    {
        if (ps.isAlive())
        {
            if (other.tag == "Fire")
            {
                StartBurning();
            }

            if (other.tag == "Rage")
            {
                if (!isBurning())
                {
                    _burnTimer = 0;
                    ChangeState(PersonStatus.Status.Raged);
                    _rageEmitter.emit = true;
                }
            }

            if (other.tag == "Water")
            {
                Debug.Log("Hit water");
                if (isBurning())
                {
                    Burn(false);
                    SetFearLevel(-50);
                }
            }

            if (other.tag == "Lightning")
            {
                ChangeState(PersonStatus.Status.Shocked);
            }

            if (other.tag == "Power")
            {
                State = ReturnState();
                if (State != PersonStatus.Status.Dead && State != PersonStatus.Status.Shocked)
                {
                    //if(FearLevel<50) ChangeState(AnimState.Calm);
                    //if(FearLevel>=50) ChangeState(AnimState.Concerned);
                    //if(FearLevel>=80) ChangeState(AnimState.Panic);
                    PowerScript other_power = other.GetComponent<PowerScript>();
                    SetFearLevel(other_power.Fear);
                }
            }

            if (other.tag == "Person")
            {
                if (!isBurning())
                {
                    AnimationScript other_anim = other.GetComponent<AnimationScript>();
                    if (other_anim.isBurning())
                    {
                        Debug.Log("Hit a Burning person");
                        StartBurning();
                    }
                    if (other_anim.ReturnState() == PersonStatus.Status.Raged)
                    {
                        ChangeState(PersonStatus.Status.Dead);
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

