using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Based on FPS Controller used in the tutorial:
/// https://www.youtube.com/watch?v=f473C43s8nE&t=390s&ab_channel=Dave%2FGameDevelopment
/// </summary>
public class CameraRotator : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float _mouseSensitivity = 400;
    [SerializeField] private float _verticalClampDegrees = 75;

    [Header("Dependencies")]
    [SerializeField] private Transform _playerBody;

    private float _xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // set initial rotation
        //transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
    }

    private void Update()
    {
        float xInput = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _mouseSensitivity;
        float yInput = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _mouseSensitivity;
        // up/down is technically x axis roation
        _xRotation -= yInput;
        // ensure we can't look past straight up or straight down
        _xRotation = Mathf.Clamp(_xRotation, -_verticalClampDegrees, _verticalClampDegrees);
        // rotation camera up/down
        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        // rotate player left/right
        _playerBody.Rotate(Vector3.up * xInput);
    }
}
