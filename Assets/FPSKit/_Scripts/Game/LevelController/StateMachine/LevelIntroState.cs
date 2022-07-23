using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Note: The difference between 'setup' and 'intro', is setup will always get calledon level load.
/// Intro is only called if it's determined that this is the player's first experience with the level.
/// (cutscenes, initialization, etc.)
/// </summary>
public class LevelIntroState : State
{
    private LevelFSM _stateMachine;
    private LevelController _controller;

    private PlayerSpawner _playerSpawner;
    private GameSession _gameSession;
    private PlayerHUD _playerHUD;
    private HUDScreen _introScreen;
    private CameraController _levelCamera;

    public LevelIntroState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;

        _playerSpawner = controller.PlayerSpawner;
        _gameSession = GameSession.Instance;
        _playerHUD = controller.LevelHUD.PlayerHUD;
        _introScreen = controller.LevelHUD.IntroScreen;
        _levelCamera = controller.CameraController;
    }

    public override void Enter()
    {
        base.Enter();
        // Debug.Log("LEVEL: Intro");

        _introScreen.Display();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        // TODO check if our cutscene is complete, if we have one. For now, just skip

        BeginLevel();
    }

    private void BeginLevel()
    {
        SpawnPlayer();
        _stateMachine.ChangeState(_stateMachine.ActiveState);
    }

    private void SpawnPlayer()
    {
        // spawn
        _controller.ActivePlayerCharacter = _playerSpawner.SpawnPlayer(_gameSession.SpawnLocation);
        _gameSession.LoadPlayerData(_controller.ActivePlayerCharacter);
        // ui
        _playerHUD.Display();
        _playerHUD.Initialize(_controller.ActivePlayerCharacter);
    }
}
