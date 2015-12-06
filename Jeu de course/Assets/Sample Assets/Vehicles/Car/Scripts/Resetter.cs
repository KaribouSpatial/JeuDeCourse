using UnityEngine;
using System.Collections;

public class Resetter : MonoBehaviour {

	private GameObject _gameManager = null;

	// Use this for initialization
	void Start () {
		_gameManager = GameObject.FindWithTag ("GameController");
	}


    private CarController _lastCollidedCar;

    // Cette fonction était utile pour dessiner en tout temps les box et s'assurer qu'il n'y avait pas de trous
    /*private void OnDrawGizmos()
    {
        BoxCollider b = GetComponent<BoxCollider>();

        Gizmos.color = Color.green;
        var corner1 = transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f);
        var corner2 = transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f);
        var corner3 = transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f);
        var corner4 = transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f);

        var corner5 = transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, b.size.z) * 0.5f);
        var corner6 = transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, b.size.z) * 0.5f);
        var corner7 = transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, -b.size.z) * 0.5f);
        var corner8 = transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, -b.size.z) * 0.5f);

        Gizmos.DrawLine(corner1, corner2);
        Gizmos.DrawLine(corner2, corner3);
        Gizmos.DrawLine(corner3, corner4);
        Gizmos.DrawLine(corner4, corner1);

        Gizmos.DrawLine(corner5, corner6);
        Gizmos.DrawLine(corner6, corner7);
        Gizmos.DrawLine(corner7, corner8);
        Gizmos.DrawLine(corner8, corner5);

        Gizmos.DrawLine(corner1, corner5);
        Gizmos.DrawLine(corner2, corner6);
        Gizmos.DrawLine(corner3, corner7);
        Gizmos.DrawLine(corner4, corner8);
    }*/

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
