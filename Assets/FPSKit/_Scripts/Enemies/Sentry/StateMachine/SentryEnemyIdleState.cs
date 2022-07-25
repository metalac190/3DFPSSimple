using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryEnemyIdleState : State
{
    private SentryEnemyFSM _stateMachine;
    private SentryEnemyController _controller;

    private Health _health;
    private PlayerDetector _playerDetector;
    private MeshRenderer _eyeRenderer;
    private Color _idleEyeColor;
    private Color _initialEyeColor;

    private const string EmissionColorPropertyName = "_EmissionColor";

    public SentryEnemyIdleState(SentryEnemyFSM stateMachine, SentryEnemyController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;

        _health = controller.Health;
        _playerDetector = controller.PlayerDetector;
        _idleEyeColor = _controller.IdleEyeColor;
        _eyeRenderer = controller.EyeRenderer;
        _initialEyeColor = controller.EyeRenderer.material.GetColor(EmissionColorPropertyName);
    }

    public override void Enter()
    {
        base.Enter();

        _health.Damaged.AddListener(OnDamaged);

        Debug.Log("STATE: Idle");
        _eyeRenderer.material.SetColor(EmissionColorPropertyName, _idleEyeColor);
    }

    public override void Exit()
    {
        base.Exit();

        _health.Damaged.RemoveListener(OnDamaged);

        _eyeRenderer.material.SetColor(EmissionColorPropertyName, _initialEyeColor);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        // if we spot the player we're idle
        if (_playerDetector.PlayerIsVisible)
        {
            _stateMachine.ChangeState(_stateMachine.AggroState);
        }
    }

    private void OnDamaged(int damage)
    {
        _stateMachine.ChangeState(_stateMachine.AggroState);
    }
}
