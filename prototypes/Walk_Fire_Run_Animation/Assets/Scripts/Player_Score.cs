using UnityEngine;
using System.Collections;

public class Player_Score : MonoBehaviour {
	public bool usingPower;
	public int souls, score;
	public float FearLevel;
	public int UnitsLeft;
	private Transform _unitRoot;
	private int _numberUnits;

	public bool isUsingPower(){
		return usingPower;
	}

	public void setUsingPower(bool set){
		usingPower=set;
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


	void CalculateFear(){
		int TotalFear=0;
		UnitsLeft=0;
		Animation_Script Unit;
		for(int i=0;i<_numberUnits;i++){
			Unit=_unitRoot.GetChild(i).GetComponent<Animation_Script>();
			if (Unit.isAlive()){
				TotalFear+=Unit.FearLevel;
				UnitsLeft++;
			}
		}
		if(UnitsLeft>0)	FearLevel=TotalFear/UnitsLeft;
	}


	// Use this for initialization
	void Start () {
		_unitRoot = transform.Find("_Units");
		_numberUnits = _unitRoot.childCount;
		score = 0;
		souls = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			Debug.Log("SCORE = " + score + "  SOULS = " + souls);
		}
		CalculateFear();
	}
}
