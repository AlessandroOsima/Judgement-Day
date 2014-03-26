using UnityEngine;
using System.Collections;

public class PersonStatus : ValidTarget
{
	//Public vars
    public enum Status { Idle, Dead, Zombie, Concerned, Panicked, Shocked, Raged, Calm };
	//This value can be used via the editor to set the startup values of various properties
	public int initialFearLevel = 0;
	public int initialScorePoints = 1;
	public int initialSoulsPoints = 2;
	public Status initialStatus;
	//Power can be used on this unit.
	public bool initialIsValidTarget = true; 
	public bool initialShouldNotBeKilled = false;

	public delegate void onStateTransition(Status previousStatus, Status newStatus);
	public event onStateTransition stateTransition;
	public delegate void onVarTransition(int previousVar, int nextVar);
	public delegate void onBoolVarTransition(bool previousVar, bool nextVar);
	public event onVarTransition soulsTransition;
	public event onVarTransition fearLevelTransition;
	public event onVarTransition scoreTransition;
	public event onBoolVarTransition isAValidTargetTransition;
	public event onBoolVarTransition shouldNotBeKilledTransition;
	public GameObject powerShieldEffect;
	public GameObject shouldNotBeKilledEffect;

	//Private Vars
    private Status unitStatus;
    private int fearLevel;                  //How much fear
    private int scorePoints;				//How much points gives         
    private int soulPoints;			    	//How much souls     
	private PowerEffect _activePower;
	private bool _isAValidTarget = true;
	private bool _shouldNotBeKilled = false;
	private AnimationScript animator;
	private UnitNavigationController navigator;

    public void Start()
    {

		refreshEvents();

		powerShieldEffect = (GameObject)Instantiate(powerShieldEffect,new Vector3(this.transform.position.x,this.transform.position.y + ((2.5f) * (1 + this.transform.localScale.x)),this.transform.position.z),Quaternion.identity);
		powerShieldEffect.transform.parent = this.transform;
		powerShieldEffect.SetActive(false);

		shouldNotBeKilledEffect = (GameObject)Instantiate(shouldNotBeKilledEffect,new Vector3(this.transform.position.x,this.transform.position.y + ((2.5f) * (1 + this.transform.localScale.x)),this.transform.position.z),Quaternion.identity);
		shouldNotBeKilledEffect.transform.parent = this.transform;
		shouldNotBeKilledEffect.SetActive(false);

		IsAValidTarget = initialIsValidTarget;
		ShouldNotBeKilled = initialShouldNotBeKilled;
		_activePower = null;

    }

	/* 
	 * Updates all the properties with their current value, this causes all the events related to the properties to be sent. 
	 * This is useful at startup (because PersonStatus is initialized before all the other scripts except for GlobalManager and LevelGUI, 
	 * scripts initialized later will not get the first event update) and when you want to send the same event message N times.
	 */
	public void refreshEvents() 
	{
		Fear = initialFearLevel;
		Score = initialScorePoints;
		Souls = initialSoulsPoints;
		UnitStatus = initialStatus;
	}

	public void Update()
	{
		if(_activePower != null)
			_activePower.deliverPowerEffects(this,animator,navigator);

		if(animator == null)
			animator = this.GetComponent<AnimationScript>();

		if(navigator == null)
			navigator = this.GetComponent<UnitNavigationController>();
	}

    public bool isAlive()
    {
        if (unitStatus == Status.Dead)
            return false;

        return true;
    }

	public bool IsAValidTarget
	{
		get
		{
			return _isAValidTarget;
		}

		set
		{
			if(value == true && _isAValidTarget == false)
			{
				powerShieldEffect.SetActive(false);

				if(isAValidTargetTransition != null)
					isAValidTargetTransition(false,true);

				canBeTargeted = true;
			}
			else if(value == false && _isAValidTarget == true)
			{
				if(ShouldNotBeKilled)
					ShouldNotBeKilled = false;

				powerShieldEffect.SetActive(true);

				if(isAValidTargetTransition != null)
					isAValidTargetTransition(true,false);

				canBeTargeted = false;
			}

			_isAValidTarget = value;
		}
	}

	public bool ShouldNotBeKilled
	{
		get
		{
			return _shouldNotBeKilled;
		}

		set
		{
			if(value == true && _shouldNotBeKilled == false)
			{
				if(!IsAValidTarget)
					IsAValidTarget = true;

				shouldNotBeKilledEffect.SetActive(true);

				if(shouldNotBeKilledTransition != null)
					shouldNotBeKilledTransition(false,true);

			}

			if(value == false && _shouldNotBeKilled == true)
			{
				shouldNotBeKilledEffect.SetActive(false);

				if(shouldNotBeKilledTransition != null)
					shouldNotBeKilledTransition(true,false);
			}

			_shouldNotBeKilled = value;
		}
	}

    public Status UnitStatus
    {
        get
        {
            return this.unitStatus;
        }
        set
        {
			if(stateTransition != null)
				stateTransition(unitStatus, value);

			if(value == Status.Dead && unitStatus != Status.Dead)
			{

				if(ShouldNotBeKilled)
				{
					//GlobalManager.globalManager.incrementScore(scorePoints);
					GlobalManager.globalManager.incrementSouls(- 2 * soulPoints);
					GlobalManager.globalManager.decrementPopulation(1);
				}
				else
				{
					GlobalManager.globalManager.incrementScore(scorePoints);
					GlobalManager.globalManager.incrementSouls(soulPoints);
					GlobalManager.globalManager.decrementPopulation(1);
				}

				unitStatus = value;

				if(isPowerActivated())
				{
					_activePower.deliverPowerEffects(this,animator,navigator);

					if(animator == null)
						animator = this.GetComponent<AnimationScript>();
					
					if(navigator == null)
						navigator = this.GetComponent<UnitNavigationController>();
					
					ActivePower = null;
				}

				//fire off the two properties transition events (so that listerners know they are going away), and destroy their effects
				ShouldNotBeKilled = false; 
				IsAValidTarget = false;
				Destroy(powerShieldEffect);
				Destroy(powerShieldEffect);
			}

			unitStatus = value;


			if(value == Status.Dead && canBeTargeted)
				canBeTargeted = false;

			/* if (this.fearLevel == 0)
                  unitStatus = value;    //Status.Idle;
             else if (this.fearLevel <= 10)
                 unitStatus = value;     //Status.Calm;
             else if (this.fearLevel <= 20)
                 unitStatus = value;     //Status.Raged;
             else if (this.fearLevel <= 40)
                 unitStatus = value;     //Status.Panicked;
             else if (this.fearLevel <= 60)
                 unitStatus = value;     //Status.Concerned;
             else if (this.fearLevel <= 80)
                 unitStatus = value;     //Status.Shocked;
             else if (this.fearLevel >= 100)
                 unitStatus = value;     //Status.Dead;
             * */
        }

    }


    public int Fear
    {
        get
        {
            return fearLevel;
        }
        set
        {
			if(value > 0 && value <= 100)
			{
				if(fearLevelTransition != null)
					fearLevelTransition(fearLevel,value);

            	fearLevel = value;
			}

			if(value <= 0)
			{
				if(fearLevelTransition != null)
					fearLevelTransition(fearLevel,0);

				fearLevel = 0;
			}

			if(value > 100)
			{
				if(fearLevelTransition != null)
					fearLevelTransition(fearLevel,100);
				
				fearLevel = 100;
			}


        }
    }

    public int Souls
    {
        get
        {
            return soulPoints;
        }
        set
        {
			if(value > 0)
			{
				if(soulsTransition != null)
					soulsTransition(soulPoints,value);

                soulPoints = value;
			}
			else
			{
				if(soulsTransition != null)
					soulsTransition(soulPoints,0);

				soulPoints = 0;
			}
        }
    }

	public PowerEffect ActivePower
	{
		get
		{
			return _activePower;
		}

		set
		{
			if((_activePower == null && value != null) || (_activePower != null && value == null))
				_activePower = value;

			if(_activePower == null)
				canBeTargeted = true;
			else 
				canBeTargeted = false;

			
		}
	}

    public int Score
    {
        get
        {
            return scorePoints;
        }
        set
        {
			if(value > 0)
			{
				if(scoreTransition != null)
					scoreTransition(scorePoints,value);

            	scorePoints = value;
			}
			else
			{
				if(scoreTransition != null)
					scoreTransition(scorePoints,0);

				scorePoints = 0;
			}
        }
    }

	public bool isPowerActivated()
	{
		if(ActivePower != null)
			return true;

		return false;
	}



}
