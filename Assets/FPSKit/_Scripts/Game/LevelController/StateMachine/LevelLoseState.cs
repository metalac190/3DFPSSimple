using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoseState : State
{
    private LevelFSM _stateMachine;

    private PlayerSpawner _playerSpawner;
    private GameSession _gameSession;
    private HUDScreen _loseScreen;

    float _respawnDelay = 0;

    public LevelLoseState(LevelFSM stateMachine, LevelController controller)
    {
        _stateMachine = stateMachine;

        _playerSpawner = controller.PlayerSpawner;
        _gameSession = GameSession.Instance;
        _respawnDelay = controller.LevelData.RespawnDelay;
        _loseScreen = controller.LevelHUD.LoseScreen;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE: Lose state");

        _gameSession.DeathCount++;
        // _loseScreen.Display();
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

        if(StateDuration >= _respawnDelay)
        {
            LevelLoader.ReloadLevel();
            return;
        }
    }
}
