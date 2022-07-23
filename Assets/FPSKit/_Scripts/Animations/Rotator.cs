using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField][Tooltip("Direction the object rotates")]
    private Vector3 _rotateDirection = new Vector3(0, 1, 0);
    [SerializeField][Tooltip("Speed of object rotation")]
    private float _rotateSpeed = 100f;

    void Update()
    {
        transform.Rotate(_rotateDirection * Time.deltaTime * _rotateSpeed);
    }
}
