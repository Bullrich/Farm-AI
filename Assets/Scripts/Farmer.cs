using UnityEngine;
using System.Collections;

public class Farmer : MovingAgent {
	public Waypoint wpHome;


	// Use this for initialization
	protected override void Start () {
		base.Start();
		WalkTo(wpHome);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	protected override void WalkTo(Waypoint destination) {
		GetComponent<Animator>().SetFloat("Walk", speed);
		base.WalkTo(destination);
	}

	protected override void WalkStop() {
		GetComponent<Animator>().SetFloat("Walk", 0f);
		base.WalkStop();
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		var candy = hit.collider.GetComponent<Candy>();
		if(candy != null) {


		}
	}
}
