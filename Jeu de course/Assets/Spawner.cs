using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject Snowball;
	public float SpawnTime;

    private float _timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		_timer += Time.deltaTime;
		if (_timer >= SpawnTime) {
			_timer = 0;
			Instantiate(Snowball, transform.position, transform.rotation);
		}
	}
}
