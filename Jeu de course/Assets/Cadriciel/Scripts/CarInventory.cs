using System;
using UnityEngine;
using System.Collections;

public class CarInventory : MonoBehaviour
{

    public float InitialProjectileSpeed;
    public PickUpScript.PowerUps AvailablePowerUp;

    private GameObject _projectilesHolder;

	// Use this for initialization
	void Start () {
        _projectilesHolder = GameObject.Find("Projectiles");	
	}
	
	// Update is called once per frame
	void Update () {
#if CROSS_PLATFORM_INPUT
		var fire = CrossPlatformInput.GetAxis("Fire1");
#else
		var fire = Input.GetAxis("Fire");
#endif
	    if (Math.Abs(fire) > 0.01 && AvailablePowerUp != PickUpScript.PowerUps.Nothing)
	    {
	        switch (AvailablePowerUp)
	        {
	            case PickUpScript.PowerUps.GreenBubble:
                    createGreenBubble();
                    break;
	            case PickUpScript.PowerUps.RedBubble:
	                break;
	            case PickUpScript.PowerUps.BlueBubble:
	                break;
	            case PickUpScript.PowerUps.Nothing:
	            default:
                    break;
	        }

	        AvailablePowerUp = PickUpScript.PowerUps.Nothing;
	    }
    }

    void createGreenBubble()
    {
        var bubble = Instantiate(Resources.Load("GreenBubble")) as GameObject;
        bubble.transform.parent = _projectilesHolder.transform;
        bubble.transform.position = this.transform.FindChild("SpecialObjectStartPoint").position;

        if (this.gameObject.rigidbody.velocity.magnitude > InitialProjectileSpeed)
        {
            bubble.rigidbody.velocity = this.gameObject.rigidbody.velocity;
            bubble.rigidbody.velocity.Scale(new Vector3(2, 2, 2));
        }
        else
        {
            var direction = bubble.transform.position - this.transform.position;
            bubble.rigidbody.velocity = direction.normalized * InitialProjectileSpeed;
        }

        bubble.GetComponent<Projectile>().Speed = bubble.rigidbody.velocity.magnitude;
    }
}
