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
	private int numTargetsInside = 0;
	private List<ValidTarget> targetsInside;

	// Use this for initialization
	void Start () 
	{
		disc = this.transform.Find ("Disc").gameObject;
		normalColor = new Color (0.29f, 0.50f, 1f); //75,243,255
		hitColor = new Color (1f, 0.15f, 0.09f);
		normalMaterial = this.renderer.sharedMaterial;
		targetsInside = new List<ValidTarget>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (targetsInside.Count > 0) 
		{
			for(int i = 0; i < targetsInside.Count; i++)
			{
				if(!targetsInside[i].canBeTargeted)
				{
					numTargetsInside -= 1;
					targetsInside.Remove(targetsInside[i]);
				}
			}

			if(numTargetsInside == 0)
			{
				this.renderer.material = normalMaterial;
				disc.renderer.material = normalMaterial;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{

		ValidTarget tg = other.gameObject.GetComponent<ValidTarget>();

		if (tg != null && tg.canBeTargeted)
		{
			numTargetsInside += 1;
			targetsInside.Add(tg);
		} 

		if (numTargetsInside > 0) 
		{
			if (hitMaterial == null)
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
	
	
	void OnTriggerExit(Collider other)
	{

		ValidTarget tg = other.gameObject.GetComponent<ValidTarget>();
		
		if (tg != null && tg.canBeTargeted)
		{
			numTargetsInside -= 1;
			targetsInside.Remove(tg);
		} 

		if(numTargetsInside == 0)
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
