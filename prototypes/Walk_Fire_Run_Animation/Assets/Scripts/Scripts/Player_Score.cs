using UnityEngine;
using System.Collections;

public class Player_Score : MonoBehaviour {
	private int souls, score;
	// Use this for initialization
	void Start () {
		score = 0;
		souls = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			print("SCORE = " + score + "  SOULS = " + souls);
		}
	}
	public int reportScore(){
		return score;
	}

	public int reportSouls(){
		return souls;
	}

	public void decrementSouls(int x){
		souls = souls - x;
	}
	public void incrementScore(int x){
		score = score + x;
	}
}
