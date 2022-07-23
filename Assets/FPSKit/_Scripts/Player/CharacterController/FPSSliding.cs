using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FPSMovement))]
public class FPSSliding : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private FPSMovementData _data;

    private FPSMovement _fpsMovement;
    private Rigidbody _rb;

    // crouch repositioners
    private Vector3 _cameraStartPosition;
    private float _startingColliderHeight;
    private Vector3 _startingColliderCenter;
    private float _crouchCameraYAdjust;
    private float _crouchColliderHeightAdjust;
    private float _crouchColliderCenterAdjust;

    private float _elapsedSlideTime;

    private float _xInput;
    private float _yInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _fpsMovement = GetComponent<FPSMovement>();

        StorePositionsForSlide();
    }

    private void Update()
    {
        _xInput = Input.GetAxisRaw("Horizontal");
        _yInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.LeftControl) 
            && (_xInput != 0 || _yInput != 0))
        {
            StartSlide();
        }

        if(Input.GetKeyUp(KeyCode.LeftControl) && _fpsMovement.IsSliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (_fpsMovement.IsSliding)
        {
            Slide();
        }
    }

    private void StartSlide()
    {
        _fpsMovement.IsSliding = true;

        //TODO shrink player
        ReducePlayerHeight();

        _elapsedSlideTime = 0;
    }

    private void Slide()
    {
        Vector3 inputDirection = (transform.forward * _yInput)
            + (transform.right * _xInput);

        // sliding normal
        if(_fpsMovement.IsOnSlope == false || _rb.velocity.y > -.1f)
        {
            _rb.AddForce(inputDirection.normalized * _data.SlideForce, ForceMode.Force);
            _elapsedSlideTime += Time.deltaTime;
        }
        // sliding down a slope
        else
        {
            _rb.AddForce(_fpsMovement.GetSlopeMoveDirection(inputDirection) 
                * _data.SlideForce, ForceMode.Force);

        }


        if (_elapsedSlideTime >= _data.MaxSlideTime)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        _fpsMovement.IsSliding = false;

        ReturnPlayerHeight();
    }

    private void StorePositionsForSlide()
    {
        _cameraStartPosition = _cameraPosition.transform.position;
        _startingColliderHeight = _capsuleCollider.height;
        _startingColliderCenter = _capsuleCollider.center;
        _crouchCameraYAdjust = _cameraPosition.transform.position.y * _data.CrouchYScale;
        _crouchColliderHeightAdjust = (_capsuleCollider.height * _data.CrouchYScale) / 2;
        _crouchColliderCenterAdjust = (_capsuleCollider.center.y * _data.CrouchYScale) / 2;
    }

    private void ReducePlayerHeight()
    {
        _cameraPosition.transform.position = new Vector3(
            _cameraPosition.transform.position.x,
            _cameraPosition.transform.position.y - _crouchCameraYAdjust,
            _cameraPosition.transform.position.z
        );
        // since we're repositioning from the center we move by half height and scale
        _capsuleCollider.height = _startingColliderHeight - _crouchColliderHeightAdjust;
        _capsuleCollider.center = new Vector3
            (0, _startingColliderCenter.y - _crouchColliderCenterAdjust, 0);
    }

    private void ReturnPlayerHeight()
    {
        _cameraPosition.transform.position = new Vector3(
            _cameraPosition.transform.position.x,
            _cameraPosition.transform.position.y + _crouchCameraYAdjust,
            _cameraPosition.transform.position.z
        );
        // since we're repositioning from the center we move by half height and scale
        _capsuleCollider.height = _startingColliderHeight;
        _capsuleCollider.center = _startingColliderCenter;
    }
}
