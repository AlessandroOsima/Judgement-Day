using UnityEngine;
using System.Collections;

public class Status : MonoBehaviour {
	private int health, fear, curiosity, random;
	public float speed = 2f;
	public GlobalManager manager;

	// Use this for initialization
	void Start () 
	{
		health = 30;
		fear = 0;
		curiosity = 0;
		manager = GameObject.Find("_PlayerStats").GetComponent<GlobalManager>(); //initializing the script
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(WaitAndPrint(2.0F));
		random = Random.Range(0,3);
		if(fear>=3){
			speed = 1f;
			movement(random);
		}
		if(fear>=6){
			speed = 2f;
			movement(random);
		}
		if(fear>=9){
			speed = 3f;
			movement(random);
		}
		if(health<=0){
			renderer.enabled = false;
		}
	}

	public int reportHealth(){
		return health;
	}

	public int reportFear(){
		return fear;
	}

	public int reportCuriosity(){
		return curiosity;
	}

	public void decreaseHealth(int x){
		health = health - x;
	}

	public void increaseFear(int x){
		fear = fear + x;
	}

	void movement(int x){
		switch(x){
		case 0:
			transform.position = transform.position + Vector3.down*speed*Time.deltaTime;
			break;
		case 1:
			transform.position = transform.position + Vector3.up*speed*Time.deltaTime;
			break;
		case 2:
			transform.position = transform.position + Vector3.left*speed*Time.deltaTime;
			break;
		case 3:
			transform.position = transform.position + Vector3.right*speed*Time.deltaTime;
			break;
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.name == "Rain")
		{
			decreaseHealth(8);
			increaseFear(3);
			//player.decrementSouls(-10);
			//player.incrementScore(50);
		}
	}

	IEnumerator WaitAndPrint(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		if(fear>=3){
			print("So strange this rain");
		}
		if(fear>=6){
			print("What's happening?");
		}
		if(fear>=9){
			print("I'm going to die!!!!!");
		}
		if(health<=0){
			print("Human dead! +10 SOULS");
		}
	}
	
	
}
