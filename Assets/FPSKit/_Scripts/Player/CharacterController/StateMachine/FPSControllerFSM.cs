using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacter))]
public class FPSControllerFSM : StateMachineMB
{
    private PlayerCharacter _controller;

    public FPSControllerIdleState IdleState { get; private set; }
    public FPSControllerWalkingState WalkingState { get; private set; }
    public FPSControllerSprintingState SprintingState { get; private set; }
    public FPSControllerAirState AirState { get; private set; }
    public FPSControllerCrouchingState CrouchingState { get; private set; }
    public FPSControllerSlidingState SlidingState { get; private set; }
    public FPSControllerWallRunningState WallRunningState { get; private set; }

    private void Awake()
    {
        // dependencies
        _controller = GetComponent<PlayerCharacter>();
        // states
        IdleState = new FPSControllerIdleState(this, _controller);
        WalkingState = new FPSControllerWalkingState(this, _controller);
        SprintingState = new FPSControllerSprintingState(this, _controller);
        AirState = new FPSControllerAirState(this, _controller);
        CrouchingState = new FPSControllerCrouchingState(this, _controller);
        SlidingState = new FPSControllerSlidingState(this, _controller);
        WallRunningState = new FPSControllerWallRunningState(this, _controller);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }
}
