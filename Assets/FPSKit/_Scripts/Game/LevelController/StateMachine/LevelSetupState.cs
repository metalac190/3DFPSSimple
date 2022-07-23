using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Note: The difference between 'setup' and 'intro', is setup will always get calledon level load.
/// Intro is only called if it's determined that this is the player's first experience with the level.
/// (cutscenes, initialization, etc.)
/// </summary>
public class LevelSetupState : State
{
    private LevelFSM _stateMachine;
    private LevelController _controller;

    private GameSession _gameSession;
    private PlayerSpawner _playerSpawner;

    private PlaytimeScreen _playtimeScreen;
    private PlayerHUD _playerHUD;

    public LevelSetupState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;

        _playerSpawner = controller.PlayerSpawner;
        _gameSession = GameSession.Instance;

        _playtimeScreen = controller.LevelHUD.PlaytimeScreen;
        _playerHUD = controller.LevelHUD.PlayerHUD;
    }

    public override void Enter()
    {
        base.Enter();

        _playtimeScreen.Display();
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
        // if this is our first attempt, leave the option to do stuff before beginning play
        if (_gameSession.IsFirstAttempt)
        {
            _gameSession.ClearGameSession();
            _gameSession.SpawnLocation = _playerSpawner.StartSpawnLocation.position;
            _stateMachine.ChangeState(_stateMachine.IntroState);
            return;
        }
        // otherwise continue play at previous checkpoint
        else
        {
            SpawnPlayer();
            _stateMachine.ChangeState(_stateMachine.ActiveState);
            return;
        }
    }

    private void SpawnPlayer()
    {
        // spawning
        _controller.ActivePlayerCharacter
            = _playerSpawner.SpawnPlayer(_gameSession.SpawnLocation);
        _gameSession.LoadPlayerData(_controller.ActivePlayerCharacter);
        // ui
        _playerHUD.Display();
        _playerHUD.Initialize(_controller.ActivePlayerCharacter);
    }
}
