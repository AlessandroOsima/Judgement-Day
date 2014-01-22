using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour 
{
	public GameObject particleExplosion;

	void Awake()
	{
	}

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
		if(other.tag == GlobalManager.npcsTag)
		{
			PersonStatus person = other.GetComponent<PersonStatus>();
			particleExplosion.SetActive(true);
			person.UnitStatus = PersonStatus.Status.Dead;
			GameObject.Destroy(this.transform.parent.gameObject,particleExplosion.particleSystem.duration);
			this.gameObject.SetActive(false); 

		}
	}
}
