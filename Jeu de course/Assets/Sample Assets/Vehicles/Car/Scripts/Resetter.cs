using UnityEngine;
using System.Collections;

public class Resetter : MonoBehaviour {

	private GameObject _gameManager = null;

	// Use this for initialization
	void Start () {
		_gameManager = GameObject.FindWithTag ("GameController");
	}

	void OnTriggerExit(Collider other)
	{
		if (other as WheelCollider == null) 
		{
			CarController car = other.transform.GetComponentInParent<CarController>();
			if (car != null) 
			{
				if (other.transform.GetComponentInParent<CarUserControlMP> ()) 
				{
					var racer = _gameManager.GetComponent<RaceManager> ();
					racer.StartCoroutine ("StartCountdown");
					car.Immobilize ();
				}
				var checkpointer = _gameManager.GetComponent<CheckpointManager> ();
				checkpointer.StartCoroutine("ResetCar", car);
			}
		}
	}
}
