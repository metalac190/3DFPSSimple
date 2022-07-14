using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSMovement : MonoBehaviour
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

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayer;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 7;
    [SerializeField] private float _sprintSpeed = 10;
    [SerializeField] private float _groundDrag = 5;
    [SerializeField][Tooltip("Lower value will trigger smoothing on smaller movements")]
    private float _movementSmoothThreshold = 4;
    private Coroutine _smoothMovementRoutine = null;
   
    [Header("Crouching")]
    [SerializeField] private float _crouchMoveSpeed = 4;
    [SerializeField]
    [Range(.1f, .9f)]
    private float _crouchYScale = .5f;
    // crouch repositioners
    private Vector3 _cameraStartPosition;
    private float _startingColliderHeight;
    private Vector3 _startingColliderCenter;
    private float _crouchCameraYAdjust;
    private float _crouchColliderHeightAdjust;
    private float _crouchColliderCenterAdjust;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 120;
    [SerializeField] private float _jumpCooldown = .25f;
    [SerializeField][Range(0,1)] private float _airMovementDampener = .4f;
    [SerializeField] private float _gravityMultiplier = 5;

    [Header("Sliding")]
    [SerializeField] private float _groundSlideSpeed = 10;
    [SerializeField] private float _downSlopeSlideSpeed = 20;
    [SerializeField][Range(1,3)]
    private float _slideIncreaseMultiplier = 1.5f;
    [SerializeField][Range(1,3)]
    private float _slopeAngleSlideMultiplier = 2f;

    private float _desiredMoveSpeed;
    private float _lastDesiredMoveSpeed;
    public bool IsSliding { get; set; }

    [Header("Slope Handling")]
    [SerializeField] private float _maxSlopeAngle = 50;
    private RaycastHit _slopeHit;
    public bool OnSlope { get; private set; }

    [Header("Wall Running")]
    [SerializeField] private float _wallRunSpeed = 8.5f;
    public bool IsWallRunning { get; set; }

    [Header("Dependencies")]
    [SerializeField] private Camera _camera;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    private float _xInput = 0;
    private float _yInput = 0;
    private float _moveSpeed;

    private bool _isGrounded = false;
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
        _isGrounded = CheckIfGrounded();
        OnSlope = CheckForSlope();
        Debug.Log("Slope: " + OnSlope);

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
        if (_isGrounded)
        {
            _rb.drag = _groundDrag;
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
            _desiredMoveSpeed = _wallRunSpeed;
        }
        // Sliding State
        else if (IsSliding)
        {
            CurrentMovementState = MovementState.Sliding;
            // if we're sliding down a slope
            if (OnSlope && _rb.velocity.y < 0.1f)
            {
                _desiredMoveSpeed = _downSlopeSlideSpeed;
            }
            // otherwise it's a ground slide
            else
            {
                _desiredMoveSpeed = _groundSlideSpeed;
            }
        }

        // Crouching State
        else if (_isGrounded && Input.GetKey(KeyCode.C))
        {
            CurrentMovementState = MovementState.Crouching;
            _desiredMoveSpeed = _crouchMoveSpeed;
        }

        // Sprint State
        else if (_isGrounded && Input.GetButton("Fire3"))
        {
            CurrentMovementState = MovementState.Sprinting;
            _desiredMoveSpeed = _sprintSpeed;
        }
        // Walking state
        else if (_isGrounded)
        {
            CurrentMovementState = MovementState.Walking;
            _desiredMoveSpeed = _walkSpeed;
        }
        else if (_isGrounded == false)
        {
            CurrentMovementState = MovementState.Air;
        }

        // check if desiredMoveSpeed has changed drastically
        SmoothMovement();
    }

    private bool CheckIfGrounded()
    {
        if (Physics.CheckSphere(transform.position, _groundCheckRadius, _groundLayer))
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

        if(Input.GetButtonDown("Jump") && _readyToJump && _isGrounded)
        {
            Debug.Log("Jump!");
            _readyToJump = false;
            Jump();
            // this will call a method after a short delay
            Invoke(nameof(ResetJump), _jumpCooldown);
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
        _camera.transform.position = new Vector3(
            _camera.transform.position.x,
            _camera.transform.position.y + _crouchCameraYAdjust,
            _camera.transform.position.z
            );
        // since we're repositioning from the center we move by half height and scale
        _capsuleCollider.height = _startingColliderHeight;
        _capsuleCollider.center = _startingColliderCenter;
    }

    private void ReducePlayerHeight()
    {
        _camera.transform.position = new Vector3(
            _camera.transform.position.x,
            _camera.transform.position.y - _crouchCameraYAdjust,
            _camera.transform.position.z
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
        if (OnSlope && _readyToJump)
        {
            _rb.AddForce(GetSlopeMoveDirection(_moveDirection) * _moveSpeed * 20, ForceMode.Force);
            // if we're on the slope and moving up, push them back down
            if(_rb.velocity.y > 0)
            {
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        // move normally if we're on the ground
        else if (_isGrounded)
        {
            _rb.AddForce(_moveDirection * _moveSpeed * 10, ForceMode.Force);
        }
        // otherwise move with air velocity
        else
        {
            Vector3 gravityForce = Vector3.down * _gravityMultiplier;
            _rb.AddForce(_moveDirection * _moveSpeed * 10 * _airMovementDampener
                + gravityForce, ForceMode.Force);
        }

        // turn off gravity while on slope
        _rb.useGravity = !OnSlope;
    }

    private void ClampSpeed()
    {
        // limiting speed on slope. Don't limit jump if we haven't jumped yet
        if (OnSlope && _readyToJump)
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

        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }

    private void StorePositionsForCrouch()
    {
        _cameraStartPosition = _camera.transform.position;
        _startingColliderHeight = _capsuleCollider.height;
        _startingColliderCenter = _capsuleCollider.center;
        _crouchCameraYAdjust = _camera.transform.position.y * _crouchYScale;
        _crouchColliderHeightAdjust = (_capsuleCollider.height * _crouchYScale) / 2;
        _crouchColliderCenterAdjust = (_capsuleCollider.center.y * _crouchYScale) / 2;
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
        if(Physics.Raycast(origin, direction, out _slopeHit, length, _groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
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
        if (Mathf.Abs(_desiredMoveSpeed - _lastDesiredMoveSpeed) > _movementSmoothThreshold
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
            if (OnSlope)
            {
                float slopeAngle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90);

                elapsedTime += Time.deltaTime * _slideIncreaseMultiplier 
                    * _slopeAngleSlideMultiplier * slopeAngleIncrease;
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
}
