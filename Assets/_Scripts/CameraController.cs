using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraPositioner;


    private void Update()
    {
        transform.position = _cameraPositioner.position;
    }
}
