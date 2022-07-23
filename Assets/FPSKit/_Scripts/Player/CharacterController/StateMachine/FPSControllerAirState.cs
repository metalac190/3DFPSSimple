using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSControllerAirState : State
{
    private FPSControllerFSM _stateMachine;
    private PlayerCharacter _controller;


    public FPSControllerAirState(FPSControllerFSM stateMachine, PlayerCharacter controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("State: Air");
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
