using UnityEngine;
using System.Collections;

public class CarManoeuver : MonoBehaviour {

	public int PointsPerManoeuver = 20;
	public float JumpMinHeight = 2;	//To be considered as manoeuver
	public float WaitTimeForFailure = 3;
	public GUIText ItemDisplay;
	public GUIText Announcement;
	public float displayTime = 2;
	private bool validManoeuver = false;
	private bool jumping = false;
	private int score = 0;
	private double waitTime = 0;
	private double wipeAnnouncementTimer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y > JumpMinHeight ) {
			Announcement.text = "Big air +" + PointsPerManoeuver + " pts!";
			wipeAnnouncementTimer = displayTime;
			jumping = true;
		}
		if (jumping && transform.position.y < 1) {
			score += PointsPerManoeuver;
			ItemDisplay.text = "Score: " + score;
			jumping = false;
		}
		waitTime -= Time.deltaTime;
		if (wipeAnnouncementTimer > 0) {
			wipeAnnouncementTimer -= Time.deltaTime;
		} else if (wipeAnnouncementTimer < 0) {
			Announcement.text = "";
			wipeAnnouncementTimer = 0;
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.attachedRigidbody != null && other.attachedRigidbody.gameObject.GetComponent<CarController>() != null && waitTime <= 0) {
			validManoeuver = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if (validManoeuver) {
			score += PointsPerManoeuver;
			ItemDisplay.text = "Score: " + score;
			Announcement.text = "Dangerous +" + PointsPerManoeuver + " pts!";
			wipeAnnouncementTimer = displayTime;
		}
		validManoeuver = false;
	}

	void OnCollisionEnter(Collision collision) {
		validManoeuver = false;
		waitTime = WaitTimeForFailure;
	}
}
