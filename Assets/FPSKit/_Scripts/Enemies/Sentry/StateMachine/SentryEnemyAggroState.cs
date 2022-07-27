using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryEnemyAggroState : State
{
    private SentryEnemyFSM _stateMachine;
    private SentryEnemyController _controller;

    private float _durationUntilReturnToIdle = 2;
    private float _playerNotSeenElapsedTime = 0;

    // projectiles
    private float _fireRate = 0;
    private float _weaponCooldownElapsed;

    private PlayerDetector _playerDetector;
    private MeshRenderer _eyeRenderer;
    private Color _aggroEyeColor;
    private Color _initialEyeColor;

    private Transform _playerTransform;
    private Rigidbody _rb;

    private const string EmissionColorPropertyName = "_EmissionColor";

    public SentryEnemyAggroState(SentryEnemyFSM stateMachine, SentryEnemyController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;

        _playerDetector = controller.PlayerDetector;
        _durationUntilReturnToIdle = controller.DurationUntilReturnToIdle;
        _aggroEyeColor = _controller.AggroEyeColor;
        _eyeRenderer = controller.EyeRenderer;
        _initialEyeColor = controller.EyeRenderer.material.GetColor(EmissionColorPropertyName);
        _rb = controller.RB;

        _fireRate = controller.FireRate;
    }

    public override void Enter()
    {
        base.Enter();

        _playerTransform = MonoBehaviour.FindObjectOfType<PlayerCharacter>().transform;
        _playerNotSeenElapsedTime = 0;

        Debug.Log("STATE: Aggro");
        _eyeRenderer.material.SetColor(EmissionColorPropertyName, _aggroEyeColor);
    }

    public override void Exit()
    {
        base.Exit();

        _eyeRenderer.material.SetColor(EmissionColorPropertyName, _initialEyeColor);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        _rb.MoveRotation(Quaternion.LookRotation(_playerTransform.position - _rb.position, Vector3.up));

        CheckIfCanShoot();
        CheckPlayerVisibility();

        // check for state change
        if (_playerNotSeenElapsedTime >= _durationUntilReturnToIdle)
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
            return;
        }
    }

    private void CheckPlayerVisibility()
    {
        if (_playerDetector.PlayerIsVisible == false)
        {
            //Debug.Log("Time player hidden: " + _playerNotSeenElapsedTime);
            _playerNotSeenElapsedTime += Time.deltaTime;
        }
        // if player has been seen, reset the timer
        else if (_playerDetector.PlayerIsVisible)
            _playerNotSeenElapsedTime = 0;
    }

    private void CheckIfCanShoot()
    {
        if (_weaponCooldownElapsed >= _fireRate)
        {
            // fire
            _controller.Shoot();
            _weaponCooldownElapsed = 0;
        }
        else
        {
            _weaponCooldownElapsed += Time.deltaTime;
        }
    }
}
