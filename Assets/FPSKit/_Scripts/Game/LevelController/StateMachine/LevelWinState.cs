using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWinState : State
{
    private LevelFSM _stateMachine;
    private LevelController _controller;

    private HUDScreen _winScreen;
    private PlayerSpawner _playerSpawner;
    private PlayerInput _playerInput;

    private GameSession _gameSession;

    public LevelWinState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;
        _controller = controller;

        _playerSpawner = controller.PlayerSpawner;
        _winScreen = controller.LevelHUD.WinScreen;
        _playerInput = controller.PlayerInput;

        _gameSession = GameSession.Instance;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("STATE: Win!");
        
        //TODO save player stats before removing
        _winScreen.Display();
        _playerInput.BackspacePressed += OnBackspacePressed;
        _playerInput.EscapePressed += OnEscapePressed;
        //TODO optionally, we could create a 'PlayerInactive' state that doesn't take input,
        // in the meantime just remove it for simplicity
        _playerSpawner.RemoveExistingPlayer(_controller.ActivePlayerCharacter);

    }

    public override void Exit()
    {
        base.Exit();

        _playerInput.BackspacePressed -= OnBackspacePressed;
        _playerInput.EscapePressed -= OnEscapePressed;

        _winScreen.Hide();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnBackspacePressed()
    {
        _gameSession.ClearGameSession();
        LevelLoader.ReloadLevel();
    }

    private void OnEscapePressed()
    {
        Application.Quit();
    }
}
