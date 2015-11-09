using UnityEngine;
using System.Collections;

public class Resetter : MonoBehaviour {

	private GameObject _gameManager = null;

	// Use this for initialization
	void Start () {
		_gameManager = GameObject.FindWithTag ("GameController");
	}


    private CarController _lastCollidedCar;

    public IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(1.0f);
        _lastCollidedCar = null;
    }

    void OnTriggerExit(Collider other)
	{
		if (!(other is WheelCollider))
		{
			var car = other.transform.GetComponentInParent<CarController>();
            if (car != null && (_lastCollidedCar == null || _lastCollidedCar != car))
            {
                _lastCollidedCar = car;
                this.StartCoroutine("ResetTimer");
				if (other.transform.GetComponentInParent<CarUserControlMP> ()) 
				{
					var racer = _gameManager.GetComponent<RaceManager> ();
					racer.StartCoroutine ("StartCountdown", false);
					car.Immobilize ();
				}
				var checkpointer = _gameManager.GetComponent<CheckpointManager> ();
				checkpointer.StartCoroutine("ResetCar", car);
			}
		}
	}
}
