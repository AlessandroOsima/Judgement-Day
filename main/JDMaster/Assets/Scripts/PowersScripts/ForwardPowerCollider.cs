using UnityEngine;
using System.Collections;

public class ForwardPowerCollider : MonoBehaviour {

	Power parent;
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider other) 
	{
		parent = this.transform.parent.gameObject.GetComponent<Power>();


		if(parent != null)
			parent.OnTriggerEnter(other);
	}
}
