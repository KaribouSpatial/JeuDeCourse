using UnityEngine;
using System.Collections;

public class SnowballKiller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<Snowball>()) {
			other.GetComponent<Snowball>().startMelting();
		}
	}
}
