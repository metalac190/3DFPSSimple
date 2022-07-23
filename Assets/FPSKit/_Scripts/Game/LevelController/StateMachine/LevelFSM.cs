using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelController))]
public class LevelFSM : StateMachineMB
{
    private LevelController _controller;

    public LevelSetupState SetupState;
    public LevelIntroState IntroState;
    public LevelActiveState ActiveState;
    public LevelPauseState PauseState;
    public LevelWinState WinState;
    public LevelLoseState LoseState;

    private PlayerSpawner _playerSpawner;
    private CameraController _cameraController;
    private GameSession _gameSession;

    private void Awake()
    {
        _controller = GetComponent<LevelController>();
        _playerSpawner = _controller.PlayerSpawner;
        _cameraController = _controller.CameraController;
        _gameSession = GameSession.Instance;
        // states
        SetupState = new LevelSetupState(this, _controller);
        IntroState = new LevelIntroState(this, _controller);
        ActiveState = new LevelActiveState(this, _controller);
        PauseState = new LevelPauseState(this);
        WinState = new LevelWinState(this, _controller);
        LoseState = new LevelLoseState(this, _controller);
    }

    protected override void OnEnable()
    {
        // when players change, leave hook to do things
        _playerSpawner.PlayerSpawned += OnPlayerSpawned;
        _playerSpawner.PlayerRemoved += OnPlayerRemoved;
    }

    protected override void OnDisable()
    {
        _playerSpawner.PlayerSpawned -= OnPlayerSpawned;
        _playerSpawner.PlayerRemoved -= OnPlayerRemoved;
    }

    private void Start()
    {
        ChangeState(SetupState);
    }

    private void OnPlayerSpawned(PlayerCharacter playerCharacter)
    {
        Debug.Log("Player Spawned");
        // turn off world camera - FPS player has one built in!
        _cameraController.gameObject.SetActive(false);

        playerCharacter.Health.Died.AddListener(OnPlayerDeath);
    }

    private void OnPlayerRemoved(PlayerCharacter playerCharacter)
    {
        Debug.Log("Player Removed");
        // reenable our world camera
        _cameraController.gameObject.SetActive(true);
        _cameraController.FollowNewTarget(playerCharacter.transform);
    }

    private void OnPlayerDeath(GameObject deathObject)
    {
        _gameSession.LastDeathPosition = deathObject.transform.position;
        _cameraController.gameObject.SetActive(true);
        _cameraController.FollowNewTarget(deathObject.transform);
    }
}
