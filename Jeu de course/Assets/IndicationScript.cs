using System;
using UnityEngine;
using System.Collections;

public class IndicationScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Joueur 1")
        {
            if (this.gameObject.name == "IndicationTriggerLeft")
            {
                GameObject leftSign = GameObject.Find("UI/TurnLeft");
                if (leftSign != null)
                {
                    leftSign.GetComponent<GUITexture>().enabled = true;
                }
            }
            else if (this.gameObject.name == "IndicationTriggerRight")
            {
                GameObject rightSign = GameObject.Find("UI/TurnRight");
                if (rightSign != null)
                {
                    rightSign.GetComponent<GUITexture>().enabled = true;
                }
            }
            else if (this.gameObject.name == "IndicationTriggerOff")
            {
                GameObject leftSign = GameObject.Find("UI/TurnLeft");
                if (leftSign != null)
                {
                    leftSign.GetComponent<GUITexture>().enabled = false;
                }
                GameObject rightSign = GameObject.Find("UI/TurnRight");
                if (rightSign != null)
                {
                    rightSign.GetComponent<GUITexture>().enabled = false;
                }
            }
        }
    }
}
