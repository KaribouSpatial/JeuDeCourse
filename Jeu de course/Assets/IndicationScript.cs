using System;
using UnityEngine;
using System.Collections;

public class IndicationScript : MonoBehaviour {

	private GameObject indications;

	// Use this for initialization
	void Start () {
		indications = GameObject.Find("UI/Indications");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
		if (collider.gameObject.name == "Joueur 1" && indications != null)
        {
            if (this.gameObject.name == "IndicationTriggerLeft")
            {
				indications.GetComponent<GUITexture>().texture = Resources.Load("Textures/arrow-left") as Texture;
				indications.GetComponent<GUITexture>().enabled = true;
            }
            else if (this.gameObject.name == "IndicationTriggerRight")
            {
				indications.GetComponent<GUITexture>().texture = Resources.Load("Textures/arrow-right") as Texture;
				Debug.Log(indications.GetComponent<GUITexture>().texture);
				indications.GetComponent<GUITexture>().enabled = true;
            }
			else if (this.gameObject.name == "IndicationTriggerSplit")
			{
				indications.GetComponent<GUITexture>().texture = Resources.Load("Textures/arrow-split") as Texture;
				indications.GetComponent<GUITexture>().enabled = true;
			}
			else if (this.gameObject.name == "IndicationTriggerOff")
            {
				indications.GetComponent<GUITexture>().enabled = false;
            }
        }
    }
}
