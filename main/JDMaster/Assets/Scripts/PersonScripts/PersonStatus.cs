using UnityEngine;
using System.Collections;

public class PersonStatus : MonoBehaviour
{
	//Public vars
    public enum Status { Idle, Dead, Concerned, Panicked, Shocked, Raged, Calm };
	public int initialFearLevel;
	public int initialScorePoints;
	public int initialSoulsPoints;
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

    public void Awake()
    {
		Fear = initialFearLevel;
		Score = initialScorePoints;
		Souls = initialSoulsPoints;
		UnitStatus = initialStatus;
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
			if(fearLevel > 0)
			{
				if(fearLevelTransition != null)
					fearLevelTransition(fearLevel,value);

            	fearLevel = value;
			}
			else
			{
				if(fearLevelTransition != null)
					fearLevelTransition(fearLevel,0);

				fearLevel = 0;
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




}
