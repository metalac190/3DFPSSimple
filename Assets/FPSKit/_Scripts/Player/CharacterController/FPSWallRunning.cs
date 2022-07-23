using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSWallRunning : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private FPSMovementData _data;
    [SerializeField] private FPSCamera _fpsCamera;
    [SerializeField] private FPSMovement _fpsMovement;
    [SerializeField] private Rigidbody _rb;

    // wall running
    private float _elapsedWallRunTime;
    private float _xInput;
    private float _yInput;

    // wall exit
    private bool _isExitingWall = false;
    private float _exitWallElapsedTime = 0;

    // detection
    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;
    public bool IsWallLeft { get; private set; }
    public bool IsWallRight { get; private set; }
    public bool IsAboveGroundDistance { get; private set; }
    private bool _isUpRunningInput = false;

    private Vector3 _previousWallNormal;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _fpsMovement = GetComponent<FPSMovement>();
    }

    private void Update()
    {
        CheckForWall();
        IsAboveGroundDistance = CheckHeightOffGround();
        // if we're on the ground don't remember any wall normals
        //TODO move this to an event, not polling
        if (_fpsMovement.IsGrounded)
            _previousWallNormal = Vector3.zero;

        HandleState();
    }

    private void FixedUpdate()
    {
        if (_fpsMovement.IsWallRunning)
        {
            DoWallRun();
        }
    }

    private void CheckForWall()
    {
        // hacky
        Vector3 start = transform.position + new Vector3(0, 1, 0);
        float distance = _data.WallCheckDistance;

        Debug.DrawRay(start, transform.right * distance);
        IsWallRight = Physics.Raycast(start, transform.right, 
            out _rightWallHit, distance, _data.WallLayer);
        Debug.DrawRay(start, transform.right * -1 * distance);
        IsWallLeft = Physics.Raycast(start, transform.right * -1,
            out _leftWallHit, distance, _data.WallLayer);
    }

    private bool CheckHeightOffGround()
    {
        Vector3 offset = new Vector3(0, .2f, 0);
        return !Physics.Raycast(transform.position + offset, Vector3.down,
            _data.MinJumpHeight, _data.GroundLayer);
    }

    private void HandleState()
    {
        // get inputs
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        if (_data.AllowDiagonalRun)
        {
            _isUpRunningInput = Input.GetButton("Fire3");
        }
        else
        {
            _isUpRunningInput = false;
        }
        //Debug.Log("Previous Wall Normal: " + _previousWallNormal);

        // State - Wall Running
        if ((IsWallLeft || IsWallRight) 
            && _yInput > 0 
            && IsAboveGroundDistance
            && _isExitingWall == false
            && CheckIsPreviousWall() == false)
        {
            Debug.Log("Wall Running");
            // if we're not already wall running, and it's not the same wall
            if(_fpsMovement.IsWallRunning == false)
                StartWallRun();
            // wall run timer
            if (_elapsedWallRunTime < _data.MaxWallRunTime)
                _elapsedWallRunTime += Time.deltaTime;
            if(_elapsedWallRunTime >= _data.MaxWallRunTime)
            {
                StopWallRun();
            }

            // wall jump if triggered
            if (Input.GetButtonDown("Jump"))
                WallJump();
        }
        // State - Wall Jump
        else if (_isExitingWall)
        {
            if (_fpsMovement.IsWallRunning)
                StopWallRun();
            
            if(_exitWallElapsedTime < _data.ExitWallDuration)
                _exitWallElapsedTime += Time.deltaTime;

            if(_exitWallElapsedTime >= _data.ExitWallDuration)
                _isExitingWall = false;
        }
        // State - Releasing
        else
        {
            if (_fpsMovement.IsWallRunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        Debug.Log("Start Wall Run");

        _fpsMovement.IsWallRunning = true;

        _elapsedWallRunTime = 0;

        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        // camera effects
        //_fpsCamera.ChangeFOV(_wallRunFOV);
        if (IsWallLeft) _fpsCamera.Tilt(-5);
        else if (IsWallRight) _fpsCamera.Tilt(5);
    }

    private void DoWallRun()
    {
        // ensure we don't fall
        _rb.useGravity = _data.UseWallGravity;

        // get wall normal from the wall we're active on
        Vector3 wallNormal = IsWallRight ? _rightWallHit.normal : _leftWallHit.normal;
        // convert move direction to parallel with wall (for curved surfaces)
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);
        // ensure we get wall direction from where player is facing
        if((transform.forward - wallForward).magnitude 
            > (transform.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        _rb.AddForce(wallForward * _data.WallRunForce, ForceMode.Force);

        // diagonal wall movement
        if (_isUpRunningInput)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 
                _data.WallDiagonalSpeed, _rb.velocity.z);
        }

        // push player towards wall so they don't fall off outward curves,
        // if they're moving towards it
        bool isHoldingTowardLeftWall = IsWallLeft && _xInput < 0;
        bool isHoldingTowardRightWall = IsWallRight && _xInput > 0;
        if (isHoldingTowardLeftWall || isHoldingTowardRightWall)
        {
            _rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        // weaken gravity while wall running
        if (_data.UseWallGravity)
            _rb.AddForce(transform.up * _data.GravityCounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        Debug.Log("Stop Wall Run");
        _fpsMovement.IsWallRunning = false;
        _elapsedWallRunTime = 0;

        // camera effects
        //_fpsCamera.ChangeFOV(_fpsCamera.InitialFOV);
        _fpsCamera.ResetTilt();

        // store the previous wall hit so that we don't reenter the same wall
        _previousWallNormal = IsWallRight ? _rightWallHit.normal : _leftWallHit.normal;
        Debug.Log("Store Wall Normal: " + _previousWallNormal);
        ExitWall();
    }

    private void WallJump()
    {
        ExitWall();

        Vector3 wallNormal = IsWallRight 
            ? _rightWallHit.normal : _leftWallHit.normal;
        // calculate
        Vector3 forceToApply = (transform.up * _data.WallJumpUpForce)
            + (wallNormal * _data.WallJumpSideForce);
        // reset y for better feeling jump
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        // apply
        _rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    private void ExitWall()
    {
        _isExitingWall = true;
        _exitWallElapsedTime = 0;
    }

    private bool CheckIsPreviousWall()
    {
        // no wall stored, it can't be previous wall
        if (_previousWallNormal == Vector3.zero) return false;
        // if previous wall is either our left or right hit
        if(IsWallRight && _previousWallNormal == _rightWallHit.normal)
        {
            return true;
        }
        else if(IsWallLeft && _previousWallNormal == _leftWallHit.normal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
    private IEnumerator SmoothlyLerpWallTilt()
    {
        // smoothly lerp movement speed to desired value
        // basically, we're smoothing out our momentums
        float elapsedTime = 0;
        float difference = Mathf.Abs(_desiredMoveSpeed - _moveSpeed);
        float startValue = _moveSpeed;
        // move towards new value
        while (elapsedTime < difference)
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
    */
}
