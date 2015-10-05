using UnityEngine;
using System.Collections;

public class MoveBox : MonoBehaviour {

	[SerializeField] public PlatformerCharacter2D character = null;

	// Update is called once per frame
	void Update () 
	{
        if (character.DrawJumpLine) {
			float posX = character.transform.position.x;
			float posY = this.transform.position.y;

			if ((character.Grounded && !Input.GetKey (KeyCode.Space))) {
				posY = character.transform.position.y + findMaxHeight ();                                
			}

			Vector3 position = new Vector3 (posX, posY, 0);
			this.transform.position = position;
		} else {
			transform.position = new Vector3(0, 0, 10);
		}
	}

	private float findMaxHeight()
	{
		float initialSpeed = (character.JumpForce + character.rigidbody2D.gravityScale * Physics.gravity.y) / character.rigidbody2D.mass * Time.fixedDeltaTime;
		float height = (initialSpeed * initialSpeed) / (character.rigidbody2D.gravityScale * Physics.gravity.y * -2);
		float lingeringHeight = character.LingeringForce * character.JumpMaxPressTime * character.JumpMaxPressTime / (3 * character.rigidbody2D.mass);
		height += lingeringHeight;
		//height += (lingeringSpeed * lingeringSpeed) / (character.rigidbody2D.gravityScale * Physics.gravity.y * -2);
		//return height - character.GetComponent<CircleCollider2D>().radius * 2;
		return height;
	}
}
