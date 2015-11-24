using UnityEngine;
using System.Collections;

public class Snowball : MonoBehaviour {

	private bool isMelting = false;

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
		if (isMelting) {
			float decrement = Time.deltaTime * 0.4f;
			transform.localScale -= new Vector3(1, 1, 1) * decrement;
			rigidbody.mass -= decrement * 50;
			if (transform.localScale.x < 1) {
				Destroy(gameObject);
			}
			if (rigidbody.mass < 1) {
				rigidbody.mass = 1;
			}
		}
	}

	public void startMelting() {
		isMelting = true;
	}
}
