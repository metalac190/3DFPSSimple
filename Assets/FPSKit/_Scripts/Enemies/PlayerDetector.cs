using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField]
    [Tooltip("Add some height to player transform so we're not staring at their feet")]
    private float _heightPadding = 1;
    [SerializeField] private LayerMask _playerLayer;

    [Header("Dependencies")]
    [SerializeField][Tooltip("This is the origin of the 'vision' ray")]
    private Transform _eyes;

    public UnityEvent PlayerDetected;
    public UnityEvent PlayerEscaped;

    public bool PlayerIsInRange { get; private set; }
    public bool PlayerIsVisible { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        // if player has entered
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player == null) { return; }

        // within range, check to see if player is in line of sight
        CheckPlayerLOS(player.transform);

        // see if player is in line of sight
        // if so, aggro
    }

    private void CheckPlayerLOS(Transform playerTransform)
    {
        // add a little bit of height here
        //Vector3 origin = 
        //Vector3 destination
        //if (Physics.Raycast
    }
}
