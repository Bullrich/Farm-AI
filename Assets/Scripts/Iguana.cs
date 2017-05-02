using UnityEngine;
using System.Collections;
using Blue.Waypoints;
using System;

public class Iguana : MovingAgent {
	public Waypoint wpWell;

	bool isFull = false, ateToday = false;		//Already ate
    enum iguanaState {
        gathering,
        returning,
        sleeping,
        waiting
    }
    iguanaState currentState;
    public WaypointManager farm;
    public float waitingTime = 1f;
    [Header("Detection settings")]
    public float detectionRange = 3f;
    public LayerMask targetMask;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		//WalkTo(wpWell);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
        STM(currentState, ref _time);
        deltaTime = Time.deltaTime;
	}

    public bool isInWell() {
        return wpWell.IsNear(transform.position);
    }

	protected override void WalkTo(Waypoint destination) {
		GetComponent<Animator>().SetFloat ("Forward", speed);
		base.WalkTo(destination);
	}

	protected override void WalkStop() {
		GetComponent<Animator>().SetFloat ("Forward", 0f);
		base.WalkStop();
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if(!isFull) {
			var candy = hit.collider.GetComponent<Candy>();
			if(candy != null) {
                if (candy.Exists()) {
                    candy.Eat();
                    isFull = ateToday = true;
                    WalkTo(wpWell);
                    currentState = iguanaState.returning;
                }
			}
		}
	}

    bool ProximityRange() {
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, detectionRange, targetMask);
        return targetInViewRadius.Length > 0;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void STM(iguanaState state, ref float _time) {
        switch (state) {
            case iguanaState.gathering:
                //WalkTo(farm.GetClosestWaypoint(transform.position));
                Move(walker.MoveToDirection(shouldSmooth));
                if (ProximityRange()) {
                    WalkTo(wpWell);
                    currentState = iguanaState.returning;
                }
                break;
            case iguanaState.returning:
                Move(walker.MoveToDirection(shouldSmooth));
                if (isInWell())
                    currentState = iguanaState.waiting;
                break;
            case iguanaState.sleeping:
                if (!isInWell())
                    Move(walker.MoveToDirection(shouldSmooth));
                else
                    WalkStop();
                break;
            case iguanaState.waiting:
                WalkStop();
                _time += deltaTime;
                if(_time > waitingTime) {
                    isFull = false;
                    if (ShouldBeActive()) {
                        WalkTo(farm.GetClosestWaypoint(transform.position));
                        currentState = iguanaState.gathering;
                    } else
                        currentState = iguanaState.sleeping;
                }
                break;
            default:
                break;
        }
    }

    public void Die() {
        gameObject.SetActive(false);
    }

    public override void ChangeCycle(DayNightCycle newCycle) {
        cycle = newCycle;
        switch (newCycle) {
            case DayNightCycle.Day:
                WalkTo(farm.GetClosestWaypoint(transform.position));
                ateToday = false;
                currentState = iguanaState.gathering;
                break;
            case DayNightCycle.Afternoon:
                break;
            case DayNightCycle.Night:
                WalkTo(wpWell);
                currentState = iguanaState.sleeping;
                if (!ateToday)
                    Die();
                break;
            default:
                break;
        }
    }

    public bool ShouldBeActive() {
        switch (cycle) {
            case DayNightCycle.Day:
                return !isNocturnal;
            case DayNightCycle.Afternoon:
                return true;
            case DayNightCycle.Night:
                return isNocturnal;
            default:
                return isNocturnal;
        }
    }
}

