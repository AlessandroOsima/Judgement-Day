using UnityEngine;
using System.Collections;

public class ShieldActivator : MonoBehaviour {
	

	public float expasionDelta = 0;
	public bool expand = false;
	public float expansionRate = 0;
	float lastStep = 0;
	// Use this for initialization
	void Start () 
	{
		lastStep = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(expand && Time.timeSinceLevelLoad > lastStep + expasionDelta)
		{
			Debug.Log("expanding");
			lastStep = Time.timeSinceLevelLoad;
			transform.localScale = new Vector3(transform.localScale.x + expansionRate, transform.localScale.y, transform.localScale.z + expansionRate);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == GlobalManager.npcsTag)
		{
			PersonStatus status = other.gameObject.GetComponent<PersonStatus>();
			status.IsAValidTarget = false;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == GlobalManager.npcsTag)
		{
			PersonStatus status = other.gameObject.GetComponent<PersonStatus>();
			status.IsAValidTarget = true;
		}
	}
}
