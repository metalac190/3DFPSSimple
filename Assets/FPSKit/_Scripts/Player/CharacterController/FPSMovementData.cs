using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_PlayerMovement_", menuName = "Unit Data/Player")]
public class FPSMovementData : ScriptableObject
{
    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayer = -1;
    public LayerMask GroundLayer => _groundLayer;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 7;
    [SerializeField] private float _sprintSpeed = 10;
    [SerializeField] private float _groundDrag = 5;
    [SerializeField][Tooltip("Lower value will trigger smoothing on smaller movements")]
    private float _movementSmoothThreshold = 3;
    // properties
    public float WalkSpeed => _walkSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float GroundDrag => _groundDrag;
    public float MovementSmoothThreshold => _movementSmoothThreshold;

    [Header("Crouching")]
    [SerializeField] private float _crouchMoveSpeed = 4;
    [SerializeField]
    [Range(.1f, .9f)]
    private float _crouchYScale = .5f;
    // properties
    public float CrouchMoveSpeed => _crouchMoveSpeed;
    public float CrouchYScale => _crouchYScale;

    [Header("Jumping")]
    [SerializeField]
    [Tooltip("Higher value makes jump higher")]
    private float _jumpForce = 14;
    [SerializeField] private float _jumpCooldown = .25f;
    [SerializeField][Range(0, 1)][Tooltip("0 is no control, 1 is full control")]
    private float _airControlAmount = .5f;
    [SerializeField]
    [Tooltip("Higher values makes gravity stronger and falling faster")]
    private float _gravityMultiplier = 12;
    // properties
    public float JumpForce => _jumpForce;
    public float JumpCooldown => _jumpCooldown;
    public float AirControlAmount => _airControlAmount;
    public float GravityMultiplier => _gravityMultiplier;

    [Header("Sliding")]
    [SerializeField] private float _maxSlideTime = .75f;
    [SerializeField] private float _slideForce = 200;
    [Tooltip("Slide speed while on flat ground")]
    [SerializeField] private float _groundSlideSpeed = 10;
    [Tooltip("Slide speed while on downwards slope")]
    [SerializeField] private float _downSlopeSlideSpeed = 20;
    [SerializeField] [Range(1, 3)]
    [Tooltip("Increased speed while sliding on slopes")]
    private float _slideIncreaseMultiplier = 1.5f;
    [SerializeField] [Range(1, 3)]
    [Tooltip("Increased speed while sliding on slopes based on steepness")]
    private float _slopeAngleSlideMultiplier = 2f;
    // properties
    public float MaxSlideTime => _maxSlideTime;
    public float SlideForce => _slideForce;
    public float GroundSlideSpeed => _groundSlideSpeed;
    public float DownSlopeSlideSpeed => _downSlopeSlideSpeed;
    public float SlideIncreaseMultiplier => _slideIncreaseMultiplier;
    public float SlopeAngleSlideMultiplier => _slopeAngleSlideMultiplier;

    [Header("Slope Handling")]
    [SerializeField] private float _maxSlopeAngle = 50;
    public float MaxSlopeAngle => _maxSlopeAngle;

    [Header("Wall Detection")]
    [SerializeField] private LayerMask _wallLayer = -1;
    [SerializeField] private float _wallCheckDistance = 1;
    [SerializeField] private float _minJumpHeight = .5f;
    // properties
    public LayerMask WallLayer => _wallLayer;
    public float WallCheckDistance => _wallCheckDistance;
    public float MinJumpHeight => _minJumpHeight;

    [Header("Wall Running")]
    [SerializeField] private float _wallRunSpeed = 8.5f;
    [SerializeField] private float _wallRunForce = 200;
    [SerializeField] private float _maxWallRunTime = .7f;
    [SerializeField] private float _wallRunFOV = 90;
    [SerializeField]
    [Tooltip("Force pulls player downwards slightly while they're wall running")]
    private bool _useWallGravity = true;
    [SerializeField]
    [Tooltip("Force that pushes player into the wall so that they 'stick'")]
    private float _gravityCounterForce = 2;

    // properties
    public float WallRunSpeed => _wallRunSpeed;
    public float WallRunForce => _wallRunForce;
    public float MaxWallRunTime => _maxWallRunTime;
    public float WallRunFOV => _wallRunFOV;
    public bool UseWallGravity => _useWallGravity;
    public float GravityCounterForce => _gravityCounterForce;

    [Header("Wall Running - Diagonal")]
    [SerializeField] private bool _allowDiagonalRun = false;
    [SerializeField] private float _wallDiagonalSpeed = 3;
    // properties
    public bool AllowDiagonalRun => _allowDiagonalRun;
    public float WallDiagonalSpeed => _wallDiagonalSpeed;

    [Header("Wall Jumping")]
    [SerializeField] private float _wallJumpUpForce = 7;
    [SerializeField] private float _wallJumpSideForce = 12;
    [SerializeField]
    [Tooltip("Time to wait before detecting wall after a jump")]
    private float _exitWallDuration = .3f;
    // properties
    public float WallJumpUpForce => _wallJumpUpForce;
    public float WallJumpSideForce => _wallJumpSideForce;
    public float ExitWallDuration => _exitWallDuration;
}
