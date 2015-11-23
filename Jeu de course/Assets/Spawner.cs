using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject snowball;
	public float spawnTime = 10;
	private float timer = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= spawnTime) {
			timer = 0;
			Instantiate(snowball, transform.position, transform.rotation);
		}
	}
}
