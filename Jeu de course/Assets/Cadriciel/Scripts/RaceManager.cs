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

	public int TimeToStart{ get{ return _timeToStart; }}

	[SerializeField]
	private int _endCountdown;

	[SerializeField]
	[RangeAttribute(1.0f,100.0f)]
	private float _rubberBandFactor;
	[SerializeField]
	[RangeAttribute(1.0f,5.0f)]
	private float _rubberBandTorqueFactor;
	[SerializeField]
	[RangeAttribute(1.0f,5.0f)]
	private float _rubberBandMaxSpeedFactor;
	[SerializeField]
	[RangeAttribute(1.0f,5.0f)]
	private float _rubberBandBrakeFactor;
	[SerializeField]
	[RangeAttribute(0.0f,5.0f)]
	private float _rubberBandDownForceFactor;

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

	IEnumerator StartCountdown(bool dead = false)
	{
		int count = _timeToStart;
		string str = dead? "Vous etes morts!\n":"";
		do 
		{
			_announcement.text = str + count.ToString();
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

		Application.LoadLevel("ending");
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
		for(int i = 0; i < CarsPositions.Length; ++i) 
		{
			if(CarsPositions[i].GetComponentInChildren<CarAIControl>() || CarsPositions[i].GetComponentInChildren<CarUserControlMP>())
			{
				CarsPositions[i].GetComponentInChildren<CarController>().moddedMaxTorque = _rubberBandTorqueFactor*(float)i * _rubberBandFactor;
				CarsPositions[i].GetComponentInChildren<CarController>().moddedMaxSpeed = _rubberBandMaxSpeedFactor*(float)i * _rubberBandFactor;
				CarsPositions[i].GetComponentInChildren<CarController>().moddedBrakePower = _rubberBandBrakeFactor*(float)i * _rubberBandFactor;
				CarsPositions[i].GetComponentInChildren<CarController>().moddedDownforce = _rubberBandDownForceFactor*(float)i * _rubberBandFactor;
			}
		}
	}
}
