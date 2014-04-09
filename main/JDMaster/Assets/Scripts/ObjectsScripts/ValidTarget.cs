using UnityEngine;
using System.Collections;

public class ValidTarget : MonoBehaviour 
{
	private bool _canBeTargeted = true;

	public bool canBeTargeted 
	{
		get 
		{
			return _canBeTargeted;
		}

		set 
		{
			_canBeTargeted = value;
		}
	}
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
