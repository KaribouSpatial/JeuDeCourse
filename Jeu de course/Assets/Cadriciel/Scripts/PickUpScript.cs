using UnityEngine;
using System.Collections;

public class PickUpScript : MonoBehaviour {

	public enum PowerUps { Nothing, GreenBubble, RedBubble, BlueBubble }
	
	public float RotationSpeed;

	public float MaxDeactivatedTime;

	public PowerUps NextPowerUp;

	private float _deactivatedTime;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);

		if (_deactivatedTime > 0) {
			_deactivatedTime -= Time.deltaTime;
			if(_deactivatedTime <= 0) {
				GetComponent<MeshRenderer> ().enabled = true;
                NextPowerUp = PowerUps.GreenBubble;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (GetComponent<MeshRenderer> ().enabled) {
			GetComponent<MeshRenderer> ().enabled = false;
			_deactivatedTime = MaxDeactivatedTime;

		    var carInventory = other.attachedRigidbody.gameObject.GetComponent<CarInventory>();
		    if (carInventory != null)
		    {
                carInventory.AvailablePowerUp = NextPowerUp;
                Debug.Log(NextPowerUp);
            }
		    NextPowerUp = PowerUps.Nothing;
		}
	}
}
