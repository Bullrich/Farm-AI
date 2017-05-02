using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

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

    public delegate void changeCycle(DayNightCycle newCycle);
    public static event changeCycle _changeCycle;

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

        _changeCycle(currentCycle);
    }

	public DayNightCycle currentCycle {
		get { return _currentCycle; }
		set { 
			if(_currentCycle != value) {
				_currentCycle = value;
				lightDay.SetActive(_currentCycle == DayNightCycle.Day);
				lightAfternoon.SetActive(_currentCycle == DayNightCycle.Afternoon);
				lightNight.SetActive(_currentCycle == DayNightCycle.Night);
                _changeCycle(_currentCycle);
			}
		}
	}

    float timer = 0f;
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        int enumLength = (int)DayNightCycle.Night;

        if(timer > cycleSeconds) {
            timer = 0;
            int currentCycleNumber = (int)currentCycle;
            currentCycle = (DayNightCycle)(currentCycleNumber + 1 > enumLength ? 0 : ++currentCycleNumber);
            if(currentCycle==DayNightCycle.Day)
                foreach (Candy candy in _allCandy)
                    candy.Grow();
        }
    }
}