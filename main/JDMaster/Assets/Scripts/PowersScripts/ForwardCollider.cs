using UnityEngine;
using System.Collections;

public class ForwardCollider : MonoBehaviour {

	BurnHouse parent;
	// Use this for initialization
	void Start () 
	{
	 	parent = this.transform.parent.gameObject.GetComponent<BurnHouse>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void OnTriggerEnter(Collider other) 
	{
		if(parent != null)
			parent.OnTriggerEnter(other);
	}
}
