using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSControllerCrouchingState : State
{
    private FPSControllerFSM _stateMachine;
    private PlayerCharacter _controller;


    public FPSControllerCrouchingState(FPSControllerFSM stateMachine, PlayerCharacter controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("State: Crouching");
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
