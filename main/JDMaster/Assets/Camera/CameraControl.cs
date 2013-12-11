using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{
	public float speed = 10; 
	public float horizontalMovement = 100;
	public float verticalMovement = 100;
	public float zoomAmount = 100;
	public float rotationSpeed = 10;
	public float rotationAngle = 10;
	public float maxAngleX = 310;
	public float minAngleX = 70;
	public float rayLenght = 1.5f;
	public bool debugDraw = false;
	public bool debugNoRaycast = false;
	float distFromMin;
	float distFromMax;

	void Start () 
	{
	}
	
	void Update () 
	{
		Move();
		Rotate();

	}
	
	void Move()
	{
		Vector3 direction = new Vector3(0,0,0);
		Vector3 origin = transform.position;
		
		direction.x = Input.GetAxis("Horizontal") * horizontalMovement;

		direction.z = Input.GetAxis("Vertical") * verticalMovement;

		
		if(!debugNoRaycast && direction.x != 0 && direction.x > 0 && Physics.Raycast(new Ray(transform.position,transform.TransformDirection(Vector3.right)),rayLenght))
		{
			direction.x = 0;

			if(debugDraw)
			Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.right),Color.red);

		} else if (debugDraw) {
			Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.right));
		}


		if(!debugNoRaycast && direction.x != 0 && direction.x < 0 &&  Physics.Raycast(new Ray(transform.position,transform.TransformDirection(Vector3.left)),rayLenght))
		{
			direction.x = 0;

			if(debugDraw)
			Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.left),Color.red);

		} else if (debugDraw) {
			Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.left));
		}

		if(!debugNoRaycast && direction.z != 0  && direction.z > 0 &&  Physics.Raycast(new Ray(transform.position,transform.TransformDirection(Vector3.forward)),rayLenght))
		{
			direction.z = 0;
			if(debugDraw)
				Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.forward),Color.red);

		} else if(debugDraw){
			Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.forward));
		}

		if(!debugNoRaycast && direction.z != 0 && direction.z < 0 && Physics.Raycast(new Ray(transform.position,transform.TransformDirection(Vector3.back)),rayLenght))
		{
			direction.z = 0;

			if(debugDraw)
				Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.back),Color.red);

		} else if(debugDraw){
			Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.back));
		}

		Vector3 movement = transform.TransformDirection(direction);

		movement.y = Input.GetAxis("Mouse ScrollWheel") * zoomAmount * -1;

		if(movement.y == 0)
			movement.y = Input.GetAxis("Zoom") * zoomAmount * -1;


		if(!debugNoRaycast && movement.y != 0 && movement.y < 0 && Physics.Raycast(new Ray(transform.position,Vector3.down),rayLenght))
		{
			movement.y = 0;
		}
		
		if(!debugNoRaycast && movement.y != 0 && movement.y > 0 && Physics.Raycast(new Ray(transform.position,Vector3.up),rayLenght))
		{
			movement.y = 0;
		}

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
			
			if(interpolatedRotation.x <= minAngleX || interpolatedRotation.x >= maxAngleX)
			{
				distFromMin = interpolatedRotation.x - minAngleX;
				distFromMax = interpolatedRotation.x - maxAngleX;

				if(distFromMin < 0)
					distFromMin *= -1;

				if(distFromMax < 0)
					distFromMax *= -1;

				if(distFromMin >= distFromMax)
			     {
				     interpolatedRotation.x = maxAngleX;
			     }
			    else
			     {
					 interpolatedRotation.x = minAngleX;
				 }
			}

			//Debug.Log(interpolatedRotation.x);

			transform.eulerAngles = interpolatedRotation;
		}
	}
	
	
}
