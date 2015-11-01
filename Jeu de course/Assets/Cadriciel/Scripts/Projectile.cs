using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

    public float Speed;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    this.rigidbody.velocity = this.rigidbody.velocity.normalized*Speed;
	}
}
