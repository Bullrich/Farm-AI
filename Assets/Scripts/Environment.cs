using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DayNightCycle {
	Day,
	Afternoon,
	Night
}

public class Environment : MonoBehaviour {


	DayNightCycle _currentCycle;
	public DayNightCycle startingCycle;
	public GameObject lightDay, lightAfternoon, lightNight;
	public Transform farm;

	public float cycleSeconds = 30f;

	List<Candy> _allCandy;

	// Use this for initialization
	void Start () {
		currentCycle = startingCycle;

		_allCandy = new List<Candy>();
		foreach(Transform xf in farm) {
			var candy = xf.GetComponent<Candy>();
			if(candy)
				_allCandy.Add(candy);
		}
	}

	DayNightCycle currentCycle {
		get { return _currentCycle; }
		set { 
			if(_currentCycle != value) {
				_currentCycle = value;
				lightDay.SetActive(_currentCycle == DayNightCycle.Day);
				lightAfternoon.SetActive(_currentCycle == DayNightCycle.Afternoon);
				lightNight.SetActive(_currentCycle == DayNightCycle.Night);
			}
		}
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
