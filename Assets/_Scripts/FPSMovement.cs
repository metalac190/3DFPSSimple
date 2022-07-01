using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSMovement : MonoBehaviour
{
    [SerializeField] private Transform _orientation;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 7;
    [SerializeField] private float _groundDrag = 5;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayer;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 120;
    [SerializeField] private float _jumpCooldown = .25f;
    [SerializeField][Range(0,1)] private float _airMovementDampener = .4f;
    [SerializeField] private float _gravityMultiplier = 5;
    

    private float _horizontalInput = 0;
    private float _verticalInput = 0;
    private bool _isGrounded = false;
    bool _readyToJump = true;

    private Vector3 _moveDirection = Vector3.zero;
    private Rigidbody _rb = null;
    private float _groundCheckRadius = .3f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        ResetJump();
    }

    private void Update()
    {
        _isGrounded = CheckIfGrounded();

        ProcessInput();
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
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump") && _readyToJump && _isGrounded)
        {
            Debug.Log("Jump!");
            _readyToJump = false;
            Jump();
            // this will call a method after a short delay
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        _moveDirection = (_orientation.forward * _verticalInput)
            + (_orientation.right * _horizontalInput);
        // move normally if we're on the ground
        if (_isGrounded)
        {
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10, ForceMode.Force);
        }
        // otherwise move with air velocity
        else
        {
            Vector3 gravityForce = Vector3.down * _gravityMultiplier;
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10 * _airMovementDampener
                + gravityForce, ForceMode.Force);
        }
    }

    private void ClampSpeed()
    {
        Vector3 velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        // limit velocity if needed
        if(velocity.magnitude > _moveSpeed)
        {
            Vector3 clampedVelocity = velocity.normalized * _moveSpeed;
            _rb.velocity = new Vector3(clampedVelocity.x, _rb.velocity.y, clampedVelocity.z);
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _groundCheckRadius);
    }
}
