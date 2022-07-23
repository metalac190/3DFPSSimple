using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSMovement : MonoBehaviour, IPushable
{
    public enum MovementState
    {
        Walking,
        Sprinting,
        Air,
        Crouching,
        Sliding,
        WallRunning
    }

    [Header("Dependencies")]
    [SerializeField] private FPSMovementData _movementData;
    [SerializeField] private Transform _cameraPositioner;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    public FPSMovementData Data => _movementData;
    // ground layer
    public bool IsGrounded { get; private set; } = false;

    // movement
    private Coroutine _smoothMovementRoutine = null;
   
    // crouching
    private Vector3 _cameraStartPosition;
    private float _startingColliderHeight;
    private Vector3 _startingColliderCenter;
    private float _crouchCameraYAdjust;
    private float _crouchColliderHeightAdjust;
    private float _crouchColliderCenterAdjust;

    // sliding
    private float _desiredMoveSpeed;
    private float _lastDesiredMoveSpeed;
    public bool IsSliding { get; set; }

    // slope handling
    private RaycastHit _slopeHit;
    public bool IsOnSlope { get; private set; }

    // wall running
    public bool IsWallRunning { get; set; }


    // input
    private float _xInput = 0;
    private float _yInput = 0;
    private float _moveSpeed;

    bool _readyToJump = true;

    public MovementState CurrentMovementState { get; private set; }
    private Vector3 _moveDirection = Vector3.zero;
    private Rigidbody _rb = null;
    private float _groundCheckRadius = .3f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        StorePositionsForCrouch();

        ResetJump();
    }



    private void Update()
    {
        IsGrounded = CheckIfGrounded();
        IsOnSlope = CheckForSlope();

        ProcessInput();
        HandleStates();
        ClampSpeed();
        SetDrag();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void SetDrag()
    {
        if (IsGrounded)
        {
            _rb.drag = Data.GroundDrag;
        }
        else
        {
            _rb.drag = 0;
        }
    }

    private void HandleStates()
    {
        // Wall Running State
        if (IsWallRunning)
        {
            CurrentMovementState = MovementState.WallRunning;
            _desiredMoveSpeed = Data.WallRunSpeed;
        }
        // Sliding State
        else if (IsSliding)
        {
            CurrentMovementState = MovementState.Sliding;
            // if we're sliding down a slope
            if (IsOnSlope && _rb.velocity.y < 0.1f)
            {
                _desiredMoveSpeed = Data.DownSlopeSlideSpeed;
            }
            // otherwise it's a ground slide
            else
            {
                _desiredMoveSpeed = Data.GroundSlideSpeed;
            }
        }

        // Crouching State
        else if (IsGrounded && Input.GetKey(KeyCode.C))
        {
            CurrentMovementState = MovementState.Crouching;
            _desiredMoveSpeed = Data.CrouchMoveSpeed;
        }

        // Sprint State
        else if (IsGrounded && Input.GetButton("Fire3"))
        {
            CurrentMovementState = MovementState.Sprinting;
            _desiredMoveSpeed = Data.SprintSpeed;
        }
        // Walking state
        else if (IsGrounded)
        {
            CurrentMovementState = MovementState.Walking;
            _desiredMoveSpeed = Data.WalkSpeed;
        }
        else if (IsGrounded == false)
        {
            CurrentMovementState = MovementState.Air;
        }

        // turn gravity off while on slope
        if (IsWallRunning == false)
            _rb.useGravity = !IsOnSlope;

        // check if desiredMoveSpeed has changed drastically
        SmoothMovement();
    }

    private bool CheckIfGrounded()
    {
        if (Physics.CheckSphere(transform.position, _groundCheckRadius, Data.GroundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ProcessInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump") && _readyToJump && IsGrounded)
        {
            Debug.Log("Jump!");
            _readyToJump = false;
            Jump();
            // this will call a method after a short delay
            Invoke(nameof(ResetJump), Data.JumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            ReducePlayerHeight();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            ReturnPlayerHeight();
        }
    }

    private void ReturnPlayerHeight()
    {
        _cameraPositioner.transform.position = new Vector3(
            _cameraPositioner.transform.position.x,
            _cameraPositioner.transform.position.y + _crouchCameraYAdjust,
            _cameraPositioner.transform.position.z
            );
        // since we're repositioning from the center we move by half height and scale
        _capsuleCollider.height = _startingColliderHeight;
        _capsuleCollider.center = _startingColliderCenter;
    }

    private void ReducePlayerHeight()
    {
        _cameraPositioner.transform.position = new Vector3(
            _cameraPositioner.transform.position.x,
            _cameraPositioner.transform.position.y - _crouchCameraYAdjust,
            _cameraPositioner.transform.position.z
            );
        // since we're repositioning from the center we move by half height and scale
        _capsuleCollider.height = _startingColliderHeight - _crouchColliderHeightAdjust;
        _capsuleCollider.center = new Vector3
            (0, _startingColliderCenter.y - _crouchColliderCenterAdjust, 0);
    }

    private void MovePlayer()
    {
        // calculate movement direction
        _moveDirection = (transform.forward * _yInput)
            + (transform.right * _xInput);
        _moveDirection.Normalize();

        // on slope and haven't exited
        if (IsOnSlope && _readyToJump)
        {
            _rb.AddForce(GetSlopeMoveDirection(_moveDirection) * _moveSpeed * 20, ForceMode.Force);
            // if we're on the slope and moving up, push them back down
            if(_rb.velocity.y > 0)
            {
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        // move normally if we're on the ground
        else if (IsGrounded)
        {
            _rb.AddForce(_moveDirection * _moveSpeed * 10, ForceMode.Force);
        }
        // otherwise move with air velocity
        else
        {
            Vector3 gravityForce = Vector3.down * Data.GravityMultiplier;
            _rb.AddForce(_moveDirection * _moveSpeed * 10 * Data.AirControlAmount
                + gravityForce, ForceMode.Force);
        }

        // turn off gravity while on slope
        _rb.useGravity = !IsOnSlope;
    }

    private void ClampSpeed()
    {
        // limiting speed on slope. Don't limit jump if we haven't jumped yet
        if (IsOnSlope && _readyToJump)
        {
            if(_rb.velocity.magnitude > _moveSpeed)
            {
                _rb.velocity = _rb.velocity.normalized * _moveSpeed;
            }
        }
        else
        {
            Vector3 velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            // limit velocity if needed
            if (velocity.magnitude > _moveSpeed)
            {
                Vector3 clampedVelocity = velocity.normalized * _moveSpeed;
                _rb.velocity = new Vector3(clampedVelocity.x, _rb.velocity.y, clampedVelocity.z);
            }
        }
    }

    private void Jump()
    {
        // reset y velocity if needed
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);

        _rb.AddForce(transform.up * Data.JumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }

    private void StorePositionsForCrouch()
    {
        _cameraStartPosition = _cameraPositioner.transform.position;
        _startingColliderHeight = _capsuleCollider.height;
        _startingColliderCenter = _capsuleCollider.center;
        _crouchCameraYAdjust = _cameraPositioner.transform.position.y * Data.CrouchYScale;
        _crouchColliderHeightAdjust = (_capsuleCollider.height * Data.CrouchYScale) / 2;
        _crouchColliderCenterAdjust = (_capsuleCollider.center.y * Data.CrouchYScale) / 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _groundCheckRadius);
    }

    private bool CheckForSlope()
    {
        // this is used to project slightly past th exact points
        float padding = .3f;
        // add a little extra to position right above ground level
        Vector3 origin = transform.position + new Vector3(0, padding, 0);
        Vector3 direction = Vector3.down;
        // shoot slightly past ground
        float length = padding * 2;
        // raycast length of half of collider (because we start at center) plus a little extra
        Debug.DrawRay(origin, direction * length);
        if(Physics.Raycast(origin, direction, out _slopeHit, length, Data.GroundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < Data.MaxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _slopeHit.normal).normalized;
    }

    private void SmoothMovement()
    {
        // if our movement has changed drastically since last frame, smooth it
        if (Mathf.Abs(_desiredMoveSpeed - _lastDesiredMoveSpeed) > Data.MovementSmoothThreshold
            && _moveSpeed != 0)
        {
            if (_smoothMovementRoutine != null)
                StopCoroutine(_smoothMovementRoutine);
            _smoothMovementRoutine = StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            _moveSpeed = _desiredMoveSpeed;
        }

        _lastDesiredMoveSpeed = _desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movement speed to desired value
        // basically, we're smoothing out our momentums
        float elapsedTime = 0;
        float difference = Mathf.Abs(_desiredMoveSpeed - _moveSpeed);
        float startValue = _moveSpeed;
        // move towards new value
        while(elapsedTime < difference)
        {
            _moveSpeed = Mathf.Lerp(startValue, _desiredMoveSpeed, elapsedTime / difference);
            // if we're on a slop, increase speed
            if (IsOnSlope)
            {
                float slopeAngle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90);

                elapsedTime += Time.deltaTime * Data.SlideIncreaseMultiplier 
                    * Data.SlopeAngleSlideMultiplier * slopeAngleIncrease;
            }
            // otherwise normal increase speed
            else
            {
                elapsedTime += Time.deltaTime;
            }

            yield return null;
        }
        // make sure we set exact end value
        _moveSpeed = _desiredMoveSpeed;
    }

    public void Push(Vector3 direction, float strength, float duration, bool canMoveDuring = true)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(direction * strength, ForceMode.Impulse);
    }
}
