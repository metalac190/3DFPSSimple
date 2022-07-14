using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSWallRunning : MonoBehaviour
{
    [Header("Wall Running")]
    [SerializeField] private LayerMask _groundLayer = -1;
    [SerializeField] private float _wallRunForce = 200;
    [SerializeField] private float _maxWallRunTime = 1;

    private float _elapsedWallRunTime;
    private float _xInput;
    private float _yInput;

    [Header("Wall Vertical")]
    [SerializeField] private bool _allowDiagonalRun = false;
    [SerializeField] private float _wallDiagonalSpeed = 3;

    [Header("Detection")]
    [SerializeField] private float _wallCheckDistance = 1;
    [SerializeField] private float _minJumpHeight = .5f;
    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;
    public bool IsWallLeft { get; private set; }
    public bool IsWallRight { get; private set; }
    public bool IsAboveGroundDistance { get; private set; }
    private bool _isUpRunningInput = false;
    private bool _isDownRunningInput = false;

    private FPSMovement _fpsMovement;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _fpsMovement = GetComponent<FPSMovement>();
    }

    private void Update()
    {
        CheckForWall();
        IsAboveGroundDistance = CheckHeightOffGround();

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
        float distance = _wallCheckDistance;

        Debug.DrawRay(start, transform.right * distance);
        IsWallRight = Physics.Raycast(start, transform.right, 
            out _rightWallHit, distance, _groundLayer);
        Debug.DrawRay(start, transform.right * -1 * distance);
        IsWallLeft = Physics.Raycast(start, transform.right * -1,
            out _leftWallHit, distance, _groundLayer);
    }

    private bool CheckHeightOffGround()
    {
        Vector3 offset = new Vector3(0, .2f, 0);
        return !Physics.Raycast(transform.position + offset, Vector3.down,
            _minJumpHeight, _groundLayer);
    }

    private void HandleState()
    {
        // get inputs
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        if (_allowDiagonalRun)
        {
            _isUpRunningInput = Input.GetButton("Fire3");
            _isDownRunningInput = Input.GetKey(KeyCode.LeftControl);
        }
        else
        {
            _isUpRunningInput = false;
            _isDownRunningInput = false;
        }

        // if we're able to wall run
        if ((IsWallLeft || IsWallRight) && _yInput > 0 
            && IsAboveGroundDistance)
        {
            // if we're not already wall running, start!
            if(_fpsMovement.IsWallRunning == false)
            {
                StartWallRun();
            }
        }
        // otherwise stop wall running
        else
        {
            if (_fpsMovement.IsWallRunning)
            {
                StopWallRun();
            }
        }
    }

    private void StartWallRun()
    {
        _fpsMovement.IsWallRunning = true;
        _elapsedWallRunTime = 0;
    }

    private void DoWallRun()
    {
        // ensure we don't fall
        _rb.useGravity = false;
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
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

        _rb.AddForce(wallForward * _wallRunForce, ForceMode.Force);

        // diagonal wall movement
        if (_isUpRunningInput)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, 
                _wallDiagonalSpeed, _rb.velocity.z);
        }
        else if (_isDownRunningInput)
        {
            _rb.velocity = new Vector3(_rb.velocity.x,
                -_wallDiagonalSpeed, _rb.velocity.z);
        }

        // push player towards wall so they don't fall off outward curves,
        // if they're moving towards it
        bool isHoldingTowardLeftWall = IsWallLeft && _xInput < 0;
        bool isHoldingTowardRightWall = IsWallRight && _xInput > 0;
        if(isHoldingTowardLeftWall || isHoldingTowardRightWall)
        {
            _rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }
    }

    private void StopWallRun()
    {
        _fpsMovement.IsWallRunning = false;
        _elapsedWallRunTime = 0;
    }
}
