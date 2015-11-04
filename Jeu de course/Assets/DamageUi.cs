using UnityEngine;
using System.Collections;

public class DamageUi : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		var myGuiTexture = GetComponent<GUITexture> ();
		int screenHeight = Screen.height;
		int screenWidth = Screen.width;
		myGuiTexture.pixelInset = new Rect (0, -screenHeight, screenWidth, screenHeight);
		myGuiTexture.color = new Color (myGuiTexture.color.r, myGuiTexture.color.g, myGuiTexture.color.b, 0.0f);
	}
}
