using UnityEngine;
using System.Collections;
using System;
using System.Reflection;


public class MinePowerDealer : BasePowerDealer
{
	//NOTE:ç You MUST use a CONVEX mesh collider on the terrain for this to work
	public GameObject mine;
	int otherCollision = 0;
    // Use this for initialization

    //COLLISION
    public override void OnTriggerEnter(Collider other)
    {

		otherCollision++;


		/*
		if((other.GetType() == typeof(BoxCollider) && other.tag == "House"))
			otherCollision--;

		if((other.GetType() == typeof(MeshCollider) && other.tag == "House"))
			otherCollision += 2;
		*/

		if(other.tag == "ShieldEffect" || other.tag == GlobalManager.npcsTag || other.tag != "Boundaries")
			otherCollision--;


  		if(other.name == "Terrain" && otherCollision <= 2)
		{
			GameObject.Instantiate(mine,godRay.transform.position,Quaternion.identity);
			otherCollision = 0;
		}
		else if(other.name == "Terrain")
		{
			otherCollision = 0;
		}     
    }
}