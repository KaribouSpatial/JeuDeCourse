using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class RaceManager : MonoBehaviour 
{


	[SerializeField]
	private GameObject _carContainer;

	[SerializeField]
	private GUIText _announcement;

	[SerializeField]
	private int _timeToStart;

	[SerializeField]
	private int _endCountdown;

    public GameObject[] CarsPositions;

	// Use this for initialization
	void Awake () 
	{
		CarActivation(false);

	}
	
	void Start()
	{
		StartCoroutine(StartCountdown());
	}

	IEnumerator StartCountdown()
	{
		int count = _timeToStart;
		do 
		{
			_announcement.text = count.ToString();
			yield return new WaitForSeconds(1.0f);
			count--;
		}
		while (count > 0);
		_announcement.text = "Partez!";
		CarActivation(true);
		yield return new WaitForSeconds(1.0f);
		_announcement.text = "";
	}

	public void EndRace(string winner)
	{
		StartCoroutine(EndRaceImpl(winner));
	}

	IEnumerator EndRaceImpl(string winner)
	{
		CarActivation(false);
		_announcement.fontSize = 20;
		int count = _endCountdown;
		do 
		{
			_announcement.text = "Victoire: " + winner + " en premiere place. Retour au titre dans " + count.ToString();
			yield return new WaitForSeconds(1.0f);
			count--;
		}
		while (count > 0);

		Application.LoadLevel("boot");
	}

	public void Announce(string announcement, float duration = 2.0f)
	{
		StartCoroutine(AnnounceImpl(announcement,duration));
	}

	IEnumerator AnnounceImpl(string announcement, float duration)
	{
		_announcement.text = announcement;
		yield return new WaitForSeconds(duration);
		_announcement.text = "";
	}

	public void CarActivation(bool activate)
	{
		foreach (CarAIControl car in _carContainer.GetComponentsInChildren<CarAIControl>(true))
		{
			car.enabled = activate;
		}
		
		foreach (CarUserControlMP car in _carContainer.GetComponentsInChildren<CarUserControlMP>(true))
		{
			car.enabled = activate;
		}

	}

    public void Update()
    {
        CarsPositions = _carContainer.GetComponentsInChildren<WaypointProgressTracker>().OrderByDescending(x => x.progressDistance).Select(x => x.gameObject).ToArray();
    }

	public void LateUpdate()
	{
		for(int i = 1; i < CarsPositions.Length; ++i) 
		{
			if(CarsPositions[i].GetComponentInChildren<CarAIControl>())
			{
				if(!CarsPositions[i].GetComponentInChildren<CarAIControl>().enabled)
					continue;
			}
			if(CarsPositions[i].GetComponentInChildren<CarUserControlMP>())
			{
				if(!CarsPositions[i].GetComponentInChildren<CarUserControlMP>().enabled)
					continue;
			}

			var car = CarsPositions[i].GetComponent<Rigidbody>();
			var force = new Vector3(car.transform.forward.x*car.velocity.x*0.01f*i, 0, car.transform.forward.z*car.velocity.z*0.01f*i);
			car.AddForce(force);
		}
	}
}
