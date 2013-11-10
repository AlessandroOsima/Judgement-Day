using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float speed =  10f;
	public float rotationSpeed = 10f;
	public float rotationAngle = 10f;
	
	private bool isJumping = false;
	private bool newJump = false;
	private float steps = 0;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		float delta = Time.deltaTime;
		Vector3 jump = Vector3.zero;
		
		//ROTATION
		float angleY = Input.GetAxis("Mouse X") * rotationAngle;
		transform.Rotate(0,angleY,0);
		
		//
		if(Input.GetKey(KeyCode.LeftShift))
		{
		}
		
		//JUMP
		if(Input.GetKey(KeyCode.Space))
		{
			
			if(!isJumping)
			{
				newJump = true;
				isJumping = true;
				Debug.Log("Jump started");
			}
		}
		
		
		if(isJumping)
		{
			float scaleCos = 0.25f;
		    float scaleSin = 0.25f;
			
			float offset = (scaleSin * Mathf.Sin(steps) + (scaleCos * Mathf.Cos(steps))) - steps/1.5f;
			jump = Vector3.up * offset;
			
			if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),1.5f) && !newJump)
			{
				jump = Vector3.zero;
				steps = 0;
				isJumping = false;
			}
			
			steps += delta * 3;
			newJump = false;
		}
	
		
		//MOVE & APPLY ROTATION
		if(Input.GetKey(KeyCode.W))
			transform.position = (transform.position + (transform.rotation * Vector3.forward * speed * delta));
		else if (Input.GetKey(KeyCode.S))
			transform.position = (transform.position + (transform.rotation * Vector3.back * speed * delta));
		else if(Input.GetKey(KeyCode.D))
			transform.position = (transform.position + (transform.rotation * Vector3.right * speed * delta));
		else if (Input.GetKey(KeyCode.A))
			transform.position = (transform.position + (transform.rotation * Vector3.left * speed * delta));
		
		//APPLY JUMP
		transform.position += jump;
	}

	void OnGUI () 
	{
		GUI.Label(new Rect (10,10,150,100), transform.position.ToString());
	}
}
