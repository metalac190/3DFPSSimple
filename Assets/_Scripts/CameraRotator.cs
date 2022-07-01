using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Based on FPS Controller used in the tutorial:
/// https://www.youtube.com/watch?v=f473C43s8nE&t=390s&ab_channel=Dave%2FGameDevelopment
/// </summary>
public class CameraRotator : MonoBehaviour
{
    [SerializeField] private float _ySensitivity = 400;
    [SerializeField] private float _xSensitivity = 400;

    [SerializeField] private Transform _orientation;

    private float _xRotation;
    private float _yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float xInput = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _xSensitivity;
        float yInput = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _ySensitivity;

        _yRotation += xInput;
        // y rotations are weird and backwards, so it's neg
        _xRotation -= yInput;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        // rotate player separately, since the player needs to stay facing forward
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
