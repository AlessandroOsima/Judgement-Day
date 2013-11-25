using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{
	public float speed = 10; 
	public float horizontalMovement = 100;
	public float frontalMovement = 100;
	public float verticalMovement = 300;
	public float rotationSpeed = 10;
	public float rotationAngle = 10;
	public float maxAngleX = 310;
	public float minAngleX = 70;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		Move();
		Rotate();

	}
	
	void Move()
	{
		Vector3 direction = new Vector3(0,0,0);
		Vector3 origin = transform.position;
		
		if(true)
			direction.x = Input.GetAxis("Horizontal") * horizontalMovement;
		
		if(true)
			direction.z = Input.GetAxis("Vertical") * frontalMovement;
		
		direction.y = 0;
		
		Vector3 movement = transform.TransformDirection(direction);
		
		if(true)
		 	movement.y = Input.GetAxis("Mouse ScrollWheel") * verticalMovement;
		
		Vector3 destination = origin + movement;
		
		
		transform.position = Vector3.MoveTowards(origin,destination,Time.deltaTime * speed);
		   
	}
	
	void Rotate()
	{
		Vector3 rotation = new Vector3(0,0,0);
		if(Input.GetAxis("Rotate enabled") > 0)
		{
			rotation.x -= Input.GetAxis("Mouse Y") * rotationAngle;
			rotation.y += Input.GetAxis("Mouse X") * rotationAngle;
			rotation.z = 0;
			
			Vector3 interpolatedRotation = Vector3.Slerp(transform.eulerAngles,transform.eulerAngles + rotation, Time.deltaTime * rotationSpeed);
			
			if(interpolatedRotation.x >= minAngleX && interpolatedRotation.x <= maxAngleX )
			{
				if(interpolatedRotation.x >= 180)
			     {
				     interpolatedRotation.x = maxAngleX;
			     }
			    else
			     {
					 interpolatedRotation.x = minAngleX;
				 }
			}
			
			transform.eulerAngles = interpolatedRotation;
		}
	}
	
	
	
}
