using UnityEngine;
using System.Collections;

public class Iguana : MovingAgent {
	public Waypoint wpWell;

	bool isFull = false;		//Already ate

	// Use this for initialization
	protected override void Start () {
		base.Start();
		WalkTo(wpWell);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
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
			


			}
		}
	}

}
