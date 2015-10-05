using UnityEngine;
using System.Collections;

public class MoveBox : MonoBehaviour {

	[SerializeField] public PlatformerCharacter2D character = null;
	[SerializeField] public GameObject testHeightObject;

	private float elapsedTime = 0.0f;
	private float maxY = 0.0f;

	void FixedUpdate()
	{
		if (character.DrawJumpLine) {

			// We use a dummy object to simulate the max height of the character
			testHeightObject.rigidbody2D.mass = character.rigidbody2D.mass;
			testHeightObject.rigidbody2D.gravityScale = character.rigidbody2D.gravityScale;
			
			float posX = character.transform.position.x;
			float posY = this.transform.position.y;
			
			if(elapsedTime == 0.0f)
			{
				testHeightObject.rigidbody2D.velocity = Vector2.zero;
				testHeightObject.transform.position = Vector3.zero;
			}
			
			if(elapsedTime < character.JumpMaxPressTime)
			{
				testHeightObject.rigidbody2D.AddForce(Vector2.Lerp(new Vector2(0.0f, character.JumpForce), Vector2.zero, elapsedTime/character.JumpMaxPressTime));
				elapsedTime += Time.fixedDeltaTime;
			}
			else if(testHeightObject.rigidbody2D.velocity.y <= 0.0f)
			{
				maxY = testHeightObject.transform.position.y;
				elapsedTime = 0;
			}
			
			if ((character.Grounded && !Input.GetKey (KeyCode.Space))) {
				// Adjust the height so it match with the feets
				posY = character.transform.position.y + maxY - character.GetComponent<BoxCollider2D>().size.y / 2 - character.GetComponent<CircleCollider2D>().radius * 4; //#MagicNumber                            
			}
			
			Vector3 position = new Vector3 (posX, posY, 0);
			this.transform.position = position;
		} else {
			transform.position = new Vector3(0, 0, 10);
		}
	}

	// Update is called once per frame
	void Update () 
	{
        
	}
}
