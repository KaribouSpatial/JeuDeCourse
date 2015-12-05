using UnityEngine;
using System.Collections;

public class CarResetter : MonoBehaviour
{

    public int TreeThreshold;
    public float MaxTime;
    public float MaxSpeed;

    private GameObject _gameManager = null;
    private float _nbTrees;
    private float _elapsedTime;

	// Use this for initialization
	void Start () {
		_gameManager = GameObject.FindWithTag ("GameController");
        // The ground count as one collision
        _nbTrees = -1;
	    _elapsedTime = -1;
	}
	
	// Update is called once per frame
	void Update () {
	    if (_elapsedTime >= 0)
	    {
	        _elapsedTime += Time.deltaTime;
	        if (transform.parent.rigidbody.velocity.magnitude > MaxSpeed)
	        {
	            _elapsedTime = 0;
	        }

            if (_elapsedTime >= MaxTime)
	        {
	            reset();
	            _elapsedTime = -1;
	        }
	    }	
	}

    void OnTriggerEnter(Collider other)
    {
        var collider = other as TerrainCollider;
        if (collider == null)
        {
            return;
        }

        ++_nbTrees;
        if (_nbTrees >= TreeThreshold && _elapsedTime < 0)
        {
            _elapsedTime = 0;
        }
    }

    void OnTriggerExit(Collider other)
    {
        var collider = other as TerrainCollider;
        if (collider == null)
        {
            return;
        }

        --_nbTrees;
        if (_nbTrees < TreeThreshold && _elapsedTime > 0)
        {
            _elapsedTime = -1;
        }
    }

    private void reset()
    {
	    var car = this.transform.parent.GetComponentInParent<CarController>();
        if (this.GetComponentInParent<CarUserControlMP> ()) 
        {
            var racer = _gameManager.GetComponent<RaceManager> ();
            racer.StartCoroutine ("StartCountdown", false);
            car.Immobilize ();
        }
        var checkpointer = _gameManager.GetComponent<CheckpointManager> ();
        checkpointer.StartCoroutine("ResetCar", car);
    }
}
