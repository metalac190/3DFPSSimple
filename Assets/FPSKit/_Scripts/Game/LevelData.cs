using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData_", menuName = "Data/Game/Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    [Tooltip("Level name used in popup window")]
    private string _levelName = "Prototype";
    [SerializeField]
    private string _levelDescription = "...";
    [SerializeField]
    [Tooltip("Duration that level popup text should stay active")]
    private float _levelNameDuration = 1.5f;
    [SerializeField]
    [Tooltip("Duration to wait before respawning player on death")]
    private float _respawnDelay = 1.5f;

    public string LevelName => _levelName;
    public string LevelDescription => _levelDescription;
    public float LevelNameDuration => _levelNameDuration;
    public float RespawnDelay => _respawnDelay;
}
