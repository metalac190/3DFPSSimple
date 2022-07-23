using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSpawner : MonoBehaviour
{
    public event Action<PlayerCharacter> PlayerSpawned;
    public event Action<PlayerCharacter> PlayerRemoved;

    [Header("Player Spawning")]

    [SerializeField]
    private Transform _startSpawnLocation;
    [SerializeField]
    private PlayerCharacter _playerPrefab;

    private PlayerCharacter _player;

    public PlayerCharacter ActivePlayer => _player;
    public Transform StartSpawnLocation => _startSpawnLocation;

    /// <summary>
    /// Spawn a new player at start position
    /// </summary>
    public PlayerCharacter SpawnPlayer(Vector3 spawnPosition)
    {
        //Debug.Log("Spawn Player");

        // if there's already a player, remove it
        PlayerCharacter existingPlayer = FindObjectOfType<PlayerCharacter>();
        if(existingPlayer != null)
        {
            RemoveExistingPlayer(existingPlayer);
        }

        _player = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);
        //TODO look into a way to pass this information before instantiating (it calls awake before initialize)

        PlayerSpawned?.Invoke(_player);

        //TODO set the camera to begin following the player

        return _player;
    }

    public void RemoveExistingPlayer(PlayerCharacter playerToRemove)
    {
        // send out notification BEFORE destroying, in case anything wants to grab something
        // from the player before it is removed
        PlayerRemoved?.Invoke(playerToRemove);
        Destroy(playerToRemove.gameObject);
    }
}
