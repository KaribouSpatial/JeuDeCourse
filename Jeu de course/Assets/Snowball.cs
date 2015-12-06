using UnityEngine;
using System.Collections;

public class Snowball : MonoBehaviour
{
    public float MeltingSpeed;
    public float MassDegradationImpact;

    private bool _isMelting = false;

	// Use this for initialization
	void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update()
    {
		if (_isMelting)
        {
			float decrement = Time.deltaTime * MeltingSpeed;
			transform.localScale -= Vector3.one * decrement;

            if (rigidbody.mass - decrement*MassDegradationImpact > 0)
            {
                rigidbody.mass -= decrement * MassDegradationImpact;
            }
            else
            {
                rigidbody.mass = 0.1f;
            }

			if (transform.localScale.x < 1)
            {
				Destroy(gameObject);
			}
			if (rigidbody.mass < 1)
            {
				rigidbody.mass = 1;
			}
		}
	}

	public void startMelting()
    {
		_isMelting = true;
	}
}
