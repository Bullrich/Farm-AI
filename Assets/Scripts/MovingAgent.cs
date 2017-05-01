using UnityEngine;
using System.Collections;

public class MovingAgent : MonoBehaviour {

	Waypoint _wpDst;
	public float speed = 2f;
	CharacterController _ctrl;

	protected virtual void Start() {
		_ctrl = GetComponent<CharacterController>();
	}

	protected virtual void WalkStop() {
		_wpDst = null;
	}

	protected virtual void WalkTo(Waypoint destination) {
		_wpDst = destination;
	}
	
	// Update is called once per frame
	protected virtual void Update () {

		Vector3 move = Vector3.zero;


		move.y = -5f * Time.deltaTime;
		_ctrl.Move(move);
	}

}
