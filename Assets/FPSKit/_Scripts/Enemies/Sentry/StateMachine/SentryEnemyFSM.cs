using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SentryEnemyController))]
public class SentryEnemyFSM : StateMachineMB
{
    private SentryEnemyController _controller;

    public SentryEnemyIdleState IdleState { get; private set; }
    public SentryEnemyAggroState AggroState { get; private set; }

    private void Awake()
    {
        // dependencies
        _controller = GetComponent<SentryEnemyController>();
        // states
        IdleState = new SentryEnemyIdleState(this, _controller);
        AggroState = new SentryEnemyAggroState(this, _controller);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }
}
