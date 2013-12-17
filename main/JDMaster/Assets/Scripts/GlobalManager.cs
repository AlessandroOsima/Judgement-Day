﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum EndGameState
{
    VictoryEasy,
    VictoryMedium,
    VictoryHard,
    VictoryCustom,
    Defeat
}

/*Global Manager stores all the global game variables, it's possible to be notified when a variable changes via the on*Changed events
 *All the variable are positive, they are set to 0 if a negative value is passed.
 */

public class GlobalManager : MonoBehaviour
{
    //Public variables
    public static string npcsTag = "Person";
    //Victory conditions
    public int easyVictory;
    public int mediumVictory;
    public int hardVictory;
    public bool standardVictoryConditions = true;
    //Private variables
    static GlobalManager _globalManager;
    public int initialSouls;
    int _souls;
    int _score;
    int _population;
    float _globalFear;
    bool firstUpdate = false;
    //Events
    public delegate void OnVarChanged(int pastVar, int newVar);
    public delegate void OnEndGame(EndGameState endGameState);
    public event OnEndGame onEndGame;
    public event OnVarChanged onSoulsChanged;
    public event OnVarChanged onPopulationChanged;
    public event OnVarChanged onScoreChanged;

	//-------------------------------MODS
	public float time;
	private float dt;
	private GameObject[] People;
	private float rate;
	private float nextwave;
	private int count;
	//-----------------------------------

    void Start()
    {
        score = 0;
		time=0f;
		rate = 20f;
		nextwave=rate;
        souls = initialSouls;
		People = GameObject.FindGameObjectsWithTag(npcsTag);
		count=4;


		for(int i = 4; i < People.Length; i++)
		{
			People[i].SetActive(false);
		}

		population = GameObject.FindGameObjectsWithTag(npcsTag).Length;
       
    }

    void Awake()
    {
        if (globalManager == null) //this way you cannot create more than one Player_Score instance
            _globalManager = this;
    }

    void Update()
    {
		dt=Time.deltaTime;
		time+=dt;
		int n  = 4;
		int j;

        if (!firstUpdate) //necessary to correctly set the GUI elements for the first time (forces On*Changed callbacks to be sent and the GUI to be updated)
        {
            score = score;
            souls = souls;
            population = population;
            firstUpdate = true;
        }
		if(time>nextwave){
			if(count<People.Length)
			{
				for(int i =count; i <count+n;i++)
				{
					if(i<People.Length)
					{
						People[i].SetActive(true);
						GlobalManager.globalManager.incrementPopulation(1);
					}
					else
					{
						//Terminar Juego
					}
				}
				count+=n;
			}

			nextwave+=rate;

			if (time>60f)
			{
				rate = 10f;
				n = 5;
			}

			if (time>120f)
			{
				rate = 5f;
				n = 7;
			}
		}

    }

    #region Properties
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
            if (value < 0)
            {
                if (onSoulsChanged != null)
                    onSoulsChanged(_souls, 0);

                _souls = 0;
            }
            else
            {
                if (onSoulsChanged != null)
                    onSoulsChanged(_souls, value);

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
            if (value < 0)
            {
                if (onScoreChanged != null)
                    onScoreChanged(_score, 0);

                _score = 0;
            }
            else
            {
                if (onScoreChanged != null)
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
            if (value <= 0)
            {
                if (onPopulationChanged != null)
                    onPopulationChanged(_population, 0);

                _population = 0;

                if (score < easyVictory && standardVictoryConditions)
                {
                    if (onEndGame != null)
                        onEndGame(EndGameState.Defeat);
                }
                else if (score >= easyVictory && standardVictoryConditions)
                {
                    if (onEndGame != null)
                        onEndGame(EndGameState.VictoryEasy);
                }
                else if (score >= mediumVictory && standardVictoryConditions)
                {
                    if (onEndGame != null)
                        onEndGame(EndGameState.VictoryMedium);
                }
                else if (score >= hardVictory && standardVictoryConditions)
                {
                    if (onEndGame != null)
                        onEndGame(EndGameState.VictoryHard);
                }

            }
            else
            {
                if (onPopulationChanged != null)
                    onPopulationChanged(_population, value);

                _population = value;
            }
        }
    }

    public float globalFear
    {
        get
        {
            return FindAverageFear();
        }
        set
        {
            if (value < 0)
                _globalFear = 0;
            else
                _globalFear = FindAverageFear();
        }
    }
    #endregion

    // Find the people's average fear
    public float FindAverageFear()
    {
        // Find all game objects with tag Person
        GameObject[] npcsList;
        npcsList = GameObject.FindGameObjectsWithTag(npcsTag);
        int totalFear = 0;
        int numberOfPeople;
        // Iterate through them and find the total fear and number of people
        for (numberOfPeople = 0; numberOfPeople < npcsList.Length; numberOfPeople++)
        {
           // Debug.Log("Paure singole: " + npcsList[numberOfPeople].GetComponent<PersonStatus>().Fear);
            totalFear += npcsList[numberOfPeople].GetComponent<PersonStatus>().Fear;
        }
        float averageFear = totalFear / numberOfPeople;
        return averageFear;
    }


    #region Increment-Decrement
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
    #endregion

    public void levelIsCompleted(EndGameState state)
    {
        if (onEndGame != null)
            onEndGame(state);
    }

}
