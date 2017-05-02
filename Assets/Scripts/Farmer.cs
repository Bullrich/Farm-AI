using UnityEngine;
using System.Collections;
using Blue.Waypoints;
using Blue.Fov;
using System;

public class Farmer : MovingAgent {
    public Waypoint wpHome;
    FieldOfView fov;
    public WaypointManager farm, ocio;

    enum mState {
        moving, paused, rotating
    }
    mState currentState = mState.moving;


    // Use this for initialization
    protected override void Start() {
        base.Start();
        fov = GetComponent<FieldOfView>();
        fov.ContinueFOV();
        WalkTo(wpHome);
    }

    Vector3 FovPos(Vector3 targetPos) {
        Vector3 toWayPoint = targetPos - transform.position;
        Vector3 direction = toWayPoint.normalized;
        Vector3 movementDelta = direction;
        return movementDelta.sqrMagnitude > toWayPoint.sqrMagnitude ? toWayPoint : movementDelta;
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        if (!fov.hasTargetInView())
            //Move(walker.MoveToDirection(shouldSmooth));
            STM(currentState, ref _time);
        else if(!fov.getTarget().GetComponent<Iguana>().isInWell())
            Move(FovPos(fov.getTarget().position));
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
        if (candy != null) {
            candy.Eat();

        } else if (hit.collider.GetComponent<Iguana>() != null) {
            hit.gameObject.SetActive(false);
        }
    }

    private void STM(mState state, ref float _time) {
        switch (state) {
            case mState.moving:
                Move(walker.MoveToDirection(shouldSmooth));
                if (!shouldSmooth && walker.closeToWaypoint()) {
                    currentState = (shouldPause ? mState.paused : mState.rotating);
                    walker.SetRotation();
                }
                break;
            case mState.paused:
                _time += deltaTime;
                if (_time >= pauseTime) {
                    currentState = mState.rotating;
                    _time = 0;
                }
                GetComponent<Animator>().SetFloat("Walk", 0f);
                break;
            case mState.rotating:
                _time = Mathf.Min(rotationTime, _time + deltaTime);
                walker.Rotate(_time, rotationTime);

                if (_time >= rotationTime) {
                    currentState = mState.moving;
                    _time = 0;
                    GetComponent<Animator>().SetFloat("Walk", speed);
                }
                break;
        }
    }

    public override void ChangeCycle(DayNightCycle newCycle) {
        if (isNocturnal)
            NocturnalCycle(newCycle);
        else
            switch (newCycle) {
                case DayNightCycle.Day:
                    shouldSmooth = true;
                    WalkTo(farm.GetClosestWaypoint(transform.position));
                    fov.ContinueFOV();
                    break;
                case DayNightCycle.Afternoon:
                    WalkTo(ocio.GetClosestWaypoint(transform.position));
                    fov.ContinueFOV();
                    shouldSmooth = false; shouldPause = true;
                    break;
                case DayNightCycle.Night:
                    WalkTo(wpHome);
                    fov.StopFOV();
                    break;
            }
    }

    public void NocturnalCycle(DayNightCycle newCycle) {
        switch (newCycle) {
            case DayNightCycle.Night:
                shouldSmooth = true;
                WalkTo(farm.GetClosestWaypoint(transform.position));
                fov.ContinueFOV();
                break;
            case DayNightCycle.Day:
                fov.ContinueFOV();
                shouldSmooth = false; shouldPause = true;
                WalkTo(ocio.GetClosestWaypoint(transform.position));
                break;
            case DayNightCycle.Afternoon:
                WalkTo(wpHome);
                fov.StopFOV();
                break;
        }
    }
}
