using UnityEngine;
using System.Collections;

public enum EndGameState
{
	Victory,
	Defeat
}

/*Global Manager stores all the global game variables, it's possible to be notified when a variable changes via the on*Changed events
 *All the variable are positive, they are set to 0 if a negative value is passed.
 */

public class GlobalManager : MonoBehaviour 
{
	//Public variables
	public static string npcsTag = "Person";
	public int easyVictory;
	public int mediumVictory;
	public int hardVictory;
	//Private variables
	static GlobalManager _globalManager;
	public int initialSouls;
	int _souls;
	int _score;
	int _population;
	int _globalFear = 0;
	bool firstUpdate = false;
	//Events
	public delegate void OnVarChanged(int pastVar, int newVar);
	public delegate void OnEndGame(EndGameState endGameState);
	public event OnEndGame onEndGame;
	public event OnVarChanged onSoulsChanged;
	public event OnVarChanged onPopulationChanged;
	public event OnVarChanged onScoreChanged;

	//Properties
	public static GlobalManager globalManager
	{
		get
		{
			return _globalManager;
		}
	}
	
	public int souls 
	{
		get
		{
			return _souls;
		}
		set
		{
			if(value < 0)
			{
				if(onSoulsChanged != null)
					onSoulsChanged(_souls,0);

				_souls = 0;
			}
			else
			{
				if(onSoulsChanged != null)
					onSoulsChanged(_souls,value);

				_souls = value;
			}
		}
	}

	public int score
	{
		get
		{
			return _score;
		}
		set
		{
			if(value < 0)
			{
				if(onScoreChanged != null)
					onScoreChanged(_score, 0);

				_score = 0;
			}
			else
			{
				if(onScoreChanged != null)
					onScoreChanged(_score, value);

				_score = value;
			}
		}
	}
	public int population
	{
		get
		{
			return _population;
		}

		set
		{
			if(value < 0)
			{
				if(onPopulationChanged != null)
					onPopulationChanged(_population,0);

				_population = 0;
			}
			else
			{
				if(onPopulationChanged != null)
					onPopulationChanged(_population,value);

				_population = value;
			}
		}
	}
	public int globalFear
	{
		get
		{
			return _globalFear;
		}
		set
		{
			if(value < 0)
				_globalFear = 0;
			else
				_globalFear = value;
		}
	}

	void Awake()
	{
		if(globalManager == null) //this way you cannot create more than one Player_Score instance
			_globalManager = this;
	}

	void Start() 
	{
		score = 0;
		souls = initialSouls;
		population = GameObject.FindGameObjectsWithTag(npcsTag).Length;

	}
	
	void Update () 
	{
		if(!firstUpdate) //necessary to correctly set the GUI elements for the first time (forces On*Changed callbacks to be sent and the GUI to be updated)
		{
			score = score;
			souls = souls;
			population = population;
			firstUpdate = true;
		}
	}
	
	public void decrementSouls(int x)
	{
		souls = souls - x;
	}

	public void incrementSouls(int x)
	{
		souls = souls + x;
	}
	
	public void decrementPopulation(int x)
	{
		population = population - x;
	}
	public void incrementPopulation(int x)
	{
		population = population + x;
	}

	public void incrementScore(int x)
	{
		score = score + x;
	}

	public void decrementScore(int x)
	{
		score = score - x;
	}

}
