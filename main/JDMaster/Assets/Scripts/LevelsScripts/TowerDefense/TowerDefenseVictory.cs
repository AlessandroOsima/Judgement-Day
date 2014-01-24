using UnityEngine;
using System.Collections;

public class TowerDefenseVictory : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GlobalManager.globalManager.onPopulationChanged += OnPopulationChanged;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnPopulationChanged(int pastVar, int newVar)
	{
		if(newVar == 0)
		{
			GlobalManager.globalManager.levelIsCompleted(EndGameState.VictoryCustom);
		}

	}
}
