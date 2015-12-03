using UnityEngine;
using System.Collections;

public class StartScreenManager : MonoBehaviour 
{
	void Awake()
	{
		Input.simulateMouseWithTouches = true;
	}

	// Update is called once per frame
	public void Play () 
	{
        Application.LoadLevel("mountainCourse");
	}
}
