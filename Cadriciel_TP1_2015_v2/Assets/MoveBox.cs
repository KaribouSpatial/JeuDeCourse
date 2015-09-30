using UnityEngine;
using System.Collections;

public class MoveBox : MonoBehaviour {

	[SerializeField] public PlatformerCharacter2D character = null;

	// Update is called once per frame
	void Update () 
	{
		if ((character.Grounded && !Input.GetKey(KeyCode.Space)) && character.DrawJumpLine) {
			float posX = character.transform.position.x;
			float posY = character.transform.position.y + findMaxHeight();
			Vector3 position = new Vector3 (posX, posY, 0);
			this.transform.position = position;
		}
	}

	private float findMaxHeight()
	{
		float initialSpeed = (character.JumpForce + character.rigidbody2D.gravityScale * Physics.gravity.y) / character.rigidbody2D.mass * Time.fixedDeltaTime;
		float height = (initialSpeed * initialSpeed) / (character.rigidbody2D.gravityScale * Physics.gravity.y * -2);
		float lingeringSpeed = (character.LingeringForce) / character.rigidbody2D.mass * character.JumpMaxPressTime;
		height += (lingeringSpeed * lingeringSpeed) / (character.rigidbody2D.gravityScale * Physics.gravity.y * -2);
		Debug.Log(height);
		return height;
	}
}
