using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {
	float dt;
	
	//---------Atributes
	public Vector3 Target; 		//Actual target of the person
	public float Speed;			//Movement speed (NavMesh speed)
	public float FearLevel;	//Determines movement behaviour
		
		
	//---------Components
	private NavMeshAgent _controller;
	private Animator _anim;
	
	
	//---------Use this for initialization
	void Start () {
		//Get Components
		_controller = GetComponent<NavMeshAgent>();
		_anim = GetComponent<Animator>();
		
		//Set Atributes
		Target = GameObject.Find("_target1").transform.position;
		
		//Set destination and speed
		_controller.destination = Target;
		SetSpeed(3f);
		SetFearLevel(0.1f);
	}
	
	
	// Update is called once per frame
	void Update () {
		//--------------Put something to update-------//
	}
	
	
	//-----------Set Procedures
	void SetFearLevel (float Fear){
		FearLevel = Fear;
		_anim.SetFloat("Speed",FearLevel);
	}
	
	void SetSpeed (float speed){
		Speed = speed;
		_controller.speed=Speed;
		_controller.acceleration=Speed;
	}
	

	
	
	//------------Colliding Triggers
	void OnTriggerEnter(Collider other){
		if (other.name=="_target1"){
			Target = GameObject.Find("_target2").transform.position;
			//Calm people
			SetSpeed(0f);
			SetFearLevel(0f);
			
			//Make them Fear
			_controller.destination = Target;
			SetSpeed(6f);
			SetFearLevel(1f);

		}
		if (other.name=="_target2"){
			Target = GameObject.Find("_target1").transform.position;
			
			//Walk calmly
			_controller.destination = Target;
			SetSpeed(2f);
			SetFearLevel(0.2f);

		}
	}
	
	
	
}
