using UnityEngine;
using System.Collections;

public class PersonStatus : MonoBehaviour {

    /*
     * DA FARE:
     * Inserire tutti gli attributi del personaggio:
     * contatori di:
     * 	Dead,       //
        Calm,   
        Concerned,  //preoccupato   
        Panic,
        Shocked,
        Idle,       //normale
        Raged       //arrabbiato
     * 
     * 
     * guardo in AnimationScript
     * 
     * numero anime
     * 
     * simile a :
     * 
     * public int souls 
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
     * 
     * 
     * 
     * */


    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
