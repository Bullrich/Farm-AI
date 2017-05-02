using UnityEngine;
using System.Collections;
using Blue.Waypoints;

//[RequireComponent(typeof(Wa))]
[SelectionBase]
public abstract class MovingAgent : MonoBehaviour {

	Waypoint _wpDst;
	public float speed = 2f;
	protected CharacterController _ctrl;
    protected WaypointWalker walker = new WaypointWalker();
    public float pauseTime, rotationTime;
    public bool shouldPause, shouldSmooth, isNocturnal;
    protected float deltaTime, _time;
    protected DayNightCycle cycle;

    protected virtual void Awake() {
        Environment._changeCycle += ChangeCycle;
    }

    protected virtual void Start() {
		_ctrl = GetComponent<CharacterController>();
	}

	protected virtual void WalkStop() {
		_wpDst = null;
	}

	protected virtual void WalkTo(Waypoint destination) {
		_wpDst = destination;
        walker.Reset(transform, speed, destination);
	}

    protected void Move(Vector3 targetPos) {
        if (_wpDst != null) {
            transform.forward = targetPos;
            _ctrl.Move(targetPos * speed * deltaTime);
        }
    }

    public abstract void ChangeCycle(DayNightCycle newCycle);
	
	// Update is called once per frame
	protected virtual void Update () {
        deltaTime = Time.deltaTime;
		Vector3 move = Vector3.zero;


		move.y = -5f * Time.deltaTime;
		//_ctrl.Move(move);
	}

}
