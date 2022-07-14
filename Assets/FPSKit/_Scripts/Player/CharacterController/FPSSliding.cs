using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FPSMovement))]
public class FPSSliding : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Transform _camera;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    private FPSMovement _fpsMovement;
    private Rigidbody _rb;

    [Header("Sliding")]
    [SerializeField] private float _maxSlideTime = .75f;
    [SerializeField] private float _slideForce = 200;
    [SerializeField][Range(.1f, .9f)]
    private float _crouchYScale = .5f;
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
        if(_fpsMovement.OnSlope == false || _rb.velocity.y > -.1f)
        {
            _rb.AddForce(inputDirection.normalized * _slideForce, ForceMode.Force);
            _elapsedSlideTime += Time.deltaTime;
        }
        // sliding down a slope
        else
        {
            _rb.AddForce(_fpsMovement.GetSlopeMoveDirection(inputDirection) 
                * _slideForce, ForceMode.Force);

        }


        if (_elapsedSlideTime >= _maxSlideTime)
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
        _cameraStartPosition = _camera.transform.position;
        _startingColliderHeight = _capsuleCollider.height;
        _startingColliderCenter = _capsuleCollider.center;
        _crouchCameraYAdjust = _camera.transform.position.y * _crouchYScale;
        _crouchColliderHeightAdjust = (_capsuleCollider.height * _crouchYScale) / 2;
        _crouchColliderCenterAdjust = (_capsuleCollider.center.y * _crouchYScale) / 2;
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
}
