using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GodRayCollider : MonoBehaviour 
{

	private GameObject disc;
	private Color normalColor;
	private Color hitColor;
	private Material hitMaterial;
	private Material normalMaterial;
	private int npcsInside = 0;
	private List<PersonStatus> personsInside;

	// Use this for initialization
	void Start () 
	{
		disc = this.transform.Find ("Disc").gameObject;
		normalColor = new Color (0.29f, 0.50f, 1f); //75,243,255
		hitColor = new Color (1f, 0.15f, 0.09f);
		normalMaterial = this.renderer.sharedMaterial;
		personsInside = new List<PersonStatus>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (personsInside.Count > 0) 
		{
			for(int i = 0; i < personsInside.Count; i++)
			{
				if(!personsInside[i].isAlive() || !personsInside[i].IsAValidTarget)
				{
					npcsInside -= 1;
					personsInside.Remove(personsInside[i]);
				}
			}

			removeFromGodRayCollider();
		}
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.tag == GlobalManager.npcsTag) 
		{
			PersonStatus ps = other.GetComponent<PersonStatus>();

			if(ps.isAlive() && ps.IsAValidTarget)
			{
				npcsInside += 1;
				personsInside.Add(ps);

				if(hitMaterial == null)
				{
					this.renderer.material.color = hitColor;
					hitMaterial = this.renderer.material;
					disc.renderer.material = hitMaterial;
				}
	        	else
				{
					this.renderer.material = hitMaterial;
					disc.renderer.material = hitMaterial;
				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == GlobalManager.npcsTag) 
		{
			PersonStatus ps = other.GetComponent<PersonStatus>();
			
			if(ps.isAlive() && ps.IsAValidTarget)
			{
				if(npcsInside > 0)
					npcsInside -= 1;

				personsInside.Remove(ps);

				removeFromGodRayCollider();
			}
		}
	}

	void removeFromGodRayCollider()
	{
		if(npcsInside == 0)
		{
			this.renderer.material = normalMaterial;
			disc.renderer.material = normalMaterial;
		}
	}

	void OnDestory()
	{
		//clean up the instanced material
		DestroyImmediate(hitMaterial);
	}
}
