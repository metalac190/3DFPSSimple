using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSControllerWalkingState : State
{
    private FPSControllerFSM _stateMachine;
    private FPSController _controller;

    public FPSControllerWalkingState(FPSControllerFSM stateMachine,
        FPSController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("State: Walking");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
}
