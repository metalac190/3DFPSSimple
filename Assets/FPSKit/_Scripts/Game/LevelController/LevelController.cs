using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private LevelData _levelData;
    [SerializeField]
    private PlayerSpawner _playerSpawner;
    [SerializeField]
    private WinTrigger _winTrigger;
    [SerializeField]
    private LevelHUD _levelHUD;
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private PlayerInput _playerInput;

    public LevelData LevelData => _levelData;
    public PlayerSpawner PlayerSpawner => _playerSpawner;
    public WinTrigger WinTrigger => _winTrigger;
    public LevelHUD LevelHUD => _levelHUD;
    public CameraController CameraController => _cameraController;
    public PlayerInput PlayerInput => _playerInput;

    private GameSession _gameSession;

    public PlayerCharacter ActivePlayerCharacter { get; set; }
} 
