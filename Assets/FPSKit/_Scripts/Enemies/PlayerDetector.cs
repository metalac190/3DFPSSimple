using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(SphereCollider))]
public class PlayerDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField]
    [Tooltip("Add some height to player transform so we're not staring at their feet")]
    private float _heightPadding = 1;
    [SerializeField]
    [Tooltip("All objects in these layers will block LOS for this enemy")]
    private LayerMask _LOSLayers;
    [SerializeField]
    [Tooltip("How often enemies check for player when player is nearby. " +
        "More frequent (lower number) is less efficient")]
    private float _LOSCheckFrequency = .5f;

    [Header("Dependencies")]
    [SerializeField][Tooltip("This is the origin of the 'vision' ray")]
    private Transform _eyes;
    // called the first time player is sighted while previously unsighted
    public event Action<GameObject> PlayerSighted;
    // called first time Player is hidden
    public event Action<GameObject> PlayerLost;
    // called the first time player enters detection radius
    public event Action<GameObject> PlayerInRange;
    // called the first time player exits detection radius
    public event Action<GameObject> PlayerLeftRange;

    public bool PlayerIsInRange { get; private set; }
    public bool PlayerIsVisible { get; private set; }

    private SphereCollider _sphereCollider;

    private Coroutine _LOSCheckRoutine = null;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // if player has entered
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player == null) { return; }

        // player is now in range
        if (PlayerIsInRange == false)
        {
            PlayerInRange?.Invoke(player.gameObject);
        }
        PlayerIsInRange = true;

        // start looking
        if (_LOSCheckRoutine != null)
            StopCoroutine(_LOSCheckRoutine);
        _LOSCheckRoutine = StartCoroutine(LOSCheckRoutine(other.transform, _LOSCheckFrequency));
    }

    private void OnTriggerExit(Collider other)
    {
        // if player has exited
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player == null) { return; }

        // player in range
        if (PlayerIsInRange == true)
        {
            PlayerLeftRange?.Invoke(player.gameObject);
        }
        PlayerIsInRange = false;
        PlayerIsVisible = false;

        // stop looking
        if (_LOSCheckRoutine != null)
            StopCoroutine(_LOSCheckRoutine);
    }

    private IEnumerator LOSCheckRoutine(Transform playerTansform, float frequency)
    {
        while (PlayerIsInRange && playerTansform != null)
        {
            //Debug.Log("Check for player...");
            CheckPlayerLOS(playerTansform);
            yield return new WaitForSeconds(frequency);
        }
    }

    private void CheckPlayerLOS(Transform playerTransform)
    {
        // add a little bit of height here
        Vector3 origin = _eyes.position;
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        RaycastHit hitInfo;
        // only check player Layer, use collider for distance
        Debug.DrawRay(origin, direction * _sphereCollider.radius, Color.red, _LOSCheckFrequency);
        if (Physics.Raycast(origin, direction, out hitInfo, _sphereCollider.radius, _LOSLayers))
        {
            // if we hit the player, the player is now detected!
            PlayerCharacter playerCharacter 
                = hitInfo.transform.gameObject.GetComponent<PlayerCharacter>();
            if (playerCharacter)
            {
                // if player was not seen previously, we now see them
                if(PlayerIsVisible == false)
                {
                    PlayerSighted?.Invoke(playerCharacter.gameObject);
                }
                PlayerIsVisible = true;
            }
            else
            {
                if (PlayerIsVisible)
                {
                    PlayerLost?.Invoke(playerCharacter.gameObject);
                }
                PlayerIsVisible = false;
            }
        }
    }
}
