using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField]
    [Tooltip("How long enemy chases player before returning to idle point")]
    private float _chaseTime = 3;

    private Vector3 _positionBeforeAggro;

    protected bool IsAggro { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        // if the player entered, aggro
        StartAggro();
    }

    private void OnTriggerExit(Collider other)
    {

    }

    protected void StartAggro()
    {

    }

    protected void StopAggro()
    {

    }
}
