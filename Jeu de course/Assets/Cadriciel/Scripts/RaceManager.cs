﻿using UnityEngine;
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
    public static string CarPositionString;
    public static float Timer;

	// Use this for initialization
	void Awake ()
	{
	    CarActivation(false);
	}
	
	void Start()
	{
		CarsPositions = new GameObject[7];
		for (int i = 0; i < _carContainer.GetComponentsInChildren<CarAIControl>().Length; i++)
		{
			CarsPositions[i] = _carContainer.GetComponentsInChildren<CarAIControl>()[i].gameObject;
		}
		CarsPositions[6] = _carContainer.GetComponentInChildren<CarUserControlMP>().gameObject;

		CarPositionString = "";

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
        this.SortCars();
        for (int i = 0; i < CarsPositions.Length; i++)
        {
            CarPositionString += CarsPositions[i].name + "," + string.Format("{0}", i + 1) + ";";
        }
        Application.LoadLevel("ending");
		//StartCoroutine(EndRaceImpl(winner));
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

    private int WaypointSorter(WaypointProgressTracker c1, WaypointProgressTracker c2)
    {
        //higher lap
        if (
            GetComponentInParent<CheckpointManager>().GetPositionFromCar(c1.GetComponentInParent<CarController>()).lap
            > GetComponentInParent<CheckpointManager>().GetPositionFromCar(c2.GetComponentInParent<CarController>()).lap)
        {
            return -1;
        }
        //higher lap
        if (
            GetComponentInParent<CheckpointManager>().GetPositionFromCar(c2.GetComponentInParent<CarController>()).lap
            > GetComponentInParent<CheckpointManager>().GetPositionFromCar(c1.GetComponentInParent<CarController>()).lap)
        {
            return 1;
        }
        //lower waypoint
        if (c2.LastWayPointObject.Key == 0 &&
            GetComponentInParent<CheckpointManager>().GetPositionFromCar(c2.GetComponentInParent<CarController>()).lap >=
            GetComponentInParent<CheckpointManager>().TotalLaps)
            return 1;
		//lower waypoint
		if (c1.LastWayPointObject.Key == 0 &&
		    GetComponentInParent<CheckpointManager>().GetPositionFromCar(c1.GetComponentInParent<CarController>()).lap >=
		    GetComponentInParent<CheckpointManager>().TotalLaps)
			return -1;
        //higher waypoint
        if (c1.LastWayPointObject.Key > c2.LastWayPointObject.Key)
        {
            return -1;
        }
        //same waypoint 
        if (c1.LastWayPointObject.Key == c2.LastWayPointObject.Key)
        {
            //same waypoint higher distance
            if (c1.LastWayPointObject.Value > c2.LastWayPointObject.Value)
                return -1;
            return 1;
        }
        return 1;
    }

    private void SortCars()
    {
        var tableau = _carContainer.GetComponentsInChildren<WaypointProgressTracker>().ToList();
        tableau.Sort(this.WaypointSorter);

        int i = 0;
        foreach (var elem in tableau)
        {
            CarsPositions[i] = tableau[i].gameObject;
            if (elem.GetComponentInParent<CarUserControlMP>())
            {
                Debug.Log(i + ", " + GetComponentInParent<CheckpointManager>().GetPositionFromCar(elem.GetComponentInParent<CarController>()).lap);
            }
            ++i;
        }
    }

    public void Update()
    {
        Timer += Time.deltaTime;
        this.SortCars();
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
