using UnityEngine;
using System.Collections;

public class Unit_Pathfinding : MonoBehaviour {
	float dt;
	float speed = 5f;
	NavMeshAgent controller;
	public Vector3 target;
	//int t_target = 1;
	// Use this for initialization
	void Start () {
		controller=GetComponent<NavMeshAgent>();
		target = GameObject.Find("_target1").transform.position;
		controller.destination = target;
		//animator = GameObject.Find("Person").GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		/*if (t_target==1){
			target = GameObject.Find("_target1").transform.position;
			controller.destination = target;
		}
		if (t_target==2){
			target = GameObject.Find("_target2").transform.position;
			controller.destination = target;
		}*/
		
	
	}
	void OnTriggerEnter(Collider other){
		if (other.name=="_target1"){
			target = GameObject.Find("_target2").transform.position;
			controller.destination = target;
			//t_target=2;
		}
		if (other.name=="_target2"){
			target = GameObject.Find("_target1").transform.position;
			controller.destination = target;
			//t_target=1;
		}
		//animator.Play("PoseLib");
	}
}
