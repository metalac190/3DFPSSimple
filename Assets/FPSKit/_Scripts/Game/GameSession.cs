using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object tracks data relevant to the current running game sessions.
/// Using SIngleton for simplified game session data. Can turn this into a save/load system
/// on file later, if needed.
/// </summary>

public class GameSession : SingletonMBPersistent<GameSession>
{
    public Vector3 SpawnLocation { get; set; } = Vector3.zero;
    public Vector3 LastDeathPosition { get; set; } = Vector3.zero;

    public int DeathCount { get; set; } = 0;
    public int FragmentCount { get; set; } = 0;
    public int KeyCount { get; set; } = 0;
    public float ElapsedTime { get; set; } = 0;

    public bool IsFirstAttempt => DeathCount <= 0;

    public void ClearGameSession()
    {
        SpawnLocation = Vector3.zero;
        LastDeathPosition = Vector3.zero;
        DeathCount = 0;
        FragmentCount = 0;
        KeyCount = 0;
        ElapsedTime = 0;
    }

    public void SavePlayerData(Vector3 spawnPoint, PlayerCharacter player)
    {
        SpawnLocation = spawnPoint;
        FragmentCount = player.Inventory.Collectibles;
        KeyCount = player.Inventory.Keys;
    }

    public void LoadPlayerData(PlayerCharacter player)
    {
        player.Inventory.Collectibles = FragmentCount;
        player.Inventory.Keys = KeyCount;
    }
}
