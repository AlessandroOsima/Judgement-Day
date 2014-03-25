using UnityEngine;
using System.Collections;

public class GodRayCollider : MonoBehaviour 
{

	private GameObject disc;
	private Color normalColor;
	private Color hitColor;
	private Material hitMaterial;
	private Material normalMaterial;
	private int npcsInside = 0;

	// Use this for initialization
	void Start () 
	{
		disc = this.transform.Find ("Disc").gameObject;
		normalColor = new Color (0.29f, 0.50f, 1f); //75,243,255
		hitColor = new Color (1f, 0.15f, 0.09f);
		normalMaterial = this.renderer.sharedMaterial;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.tag == GlobalManager.npcsTag) 
		{
			npcsInside += 1;
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

	void OnTriggerExit(Collider other)
	{
		if (other.tag == GlobalManager.npcsTag) 
		{
			npcsInside -= 1;

			if(npcsInside == 0)
			{
				this.renderer.material = normalMaterial;
				disc.renderer.material = normalMaterial;
			}
		}
	}

	void OnDestory()
	{
		//clean up the instanced material
		DestroyImmediate(hitMaterial);
	}
}
