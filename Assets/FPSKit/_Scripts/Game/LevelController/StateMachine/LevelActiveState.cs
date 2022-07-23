using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActiveState : State
{
    private LevelFSM _stateMachine;
    private LevelController _controller;

    private WinTrigger _winTrigger;

    private GameSession _gameSession;
    private PlaytimeScreen _playtimeScreen;
    private PlayerInput _playerInput;

    private float _elapsedTime;


    public LevelActiveState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;

        _winTrigger = controller.WinTrigger;
        _gameSession = GameSession.Instance;
        _playtimeScreen = controller.LevelHUD.PlaytimeScreen;
        _playerInput = controller.PlayerInput;
    }

    public override void Enter()
    {
        base.Enter();

        // Debug.Log("LEVEL: Active");
        _winTrigger.Won.AddListener(OnPlayerEnteredWin);
        // pull this from active player since it's not spawned before level load
        
        _controller.ActivePlayerCharacter.Health.Died.AddListener(OnPlayerDied);
        _playerInput.EscapePressed += OnEscapePressed;
        // load elapsed time from data
        _elapsedTime = _gameSession.ElapsedTime;
    }

    public override void Exit()
    {
        base.Exit();

        _winTrigger.Won.RemoveListener(OnPlayerEnteredWin);
        _controller.ActivePlayerCharacter.Health.Died.RemoveListener(OnPlayerDied);
        _playerInput.EscapePressed -= OnEscapePressed;

        // save elapsed time to data
        _gameSession.ElapsedTime = _elapsedTime;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        _elapsedTime += Time.deltaTime;
        _playtimeScreen.IncrementPlaytimeDisplay(_elapsedTime);
    }

    private void OnPlayerEnteredWin()
    {
        _stateMachine.ChangeState(_stateMachine.WinState);
    }

    private void OnPlayerDied(GameObject deathObject)
    {
        _stateMachine.ChangeState(_stateMachine.LoseState);
    }

    private void OnCancelPressed()
    {
        // reset level data. Make this clear to player in the future, and consider putting in menus
        _gameSession.ClearGameSession();
        LevelLoader.ReloadLevel();
    }

    private void OnEscapePressed()
    {
        //TODO leave hook for main menu
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
