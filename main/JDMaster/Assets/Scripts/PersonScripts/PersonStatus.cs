using UnityEngine;
using System.Collections;

public class PersonStatus : MonoBehaviour
{
	//Public vars
    public enum Status { Idle, Dead, Concerned, Panicked, Shocked, Raged, Calm };
	public int initialFearLevel = 0;
	public int initialScorePoints = 1;
	public int initialSoulsPoints = 2;
	public Status initialStatus;

	public delegate void onStateTransition(Status previousStatus, Status newStatus);
	public event onStateTransition stateTransition;
	public delegate void onVarTransition(int previousVar, int nextVar);
	public event onVarTransition soulsTransition;
	public event onVarTransition fearLevelTransition;
	public event onVarTransition scoreTransition;

	//Private Vars
    private Status unitStatus;
    private int fearLevel;                  //How much fear
    private int scorePoints;				//How much points gives         
    private int soulPoints;			    	//How much souls     
	private Power _activePower;
	private AnimationScript animator;
	private UnitNavigationController navigator;

    public void Start()
    {
		refreshEvents();
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
		if(animator == null)
			animator = this.GetComponent<AnimationScript>();

		if(navigator == null)
			navigator = this.GetComponent<UnitNavigationController>();
		
		if(_activePower != null)
			_activePower.deliverPowerEffects(this,animator,navigator);
	}

    public bool isAlive()
    {
        if (unitStatus == Status.Dead)
            return false;

        return true;
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
				GlobalManager.globalManager.decrementPopulation(1);
				GlobalManager.globalManager.incrementScore(scorePoints);
				GlobalManager.globalManager.incrementSouls(soulPoints);

				unitStatus = value;

				if(isPowerActivated())
				{
					if(animator == null)
						animator = this.GetComponent<AnimationScript>();
					
					if(navigator == null)
						navigator = this.GetComponent<UnitNavigationController>();

					_activePower.deliverPowerEffects(this,animator,navigator);
					
					ActivePower = null;
				}
			}

			unitStatus = value;

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

	public Power ActivePower
	{
		get
		{
			return _activePower;
		}

		set
		{
			if((_activePower == null && value != null) || (_activePower != null && value == null))
				_activePower = value;
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
