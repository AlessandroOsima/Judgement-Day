using UnityEngine;
using System.Collections;

public class LoadingStrings : MonoBehaviour 
{
    public UILabel loadingString;

    private string[] loadingStrings = {"God In Progress...", "Building The Apocalypse...", "Running Away...", "Shaming Zeus..."};

	// Use this for initialization
	void Start () 
    {
       loadingString.text = loadingStrings[Random.Range(0, 3)];
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
