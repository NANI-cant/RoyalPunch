using System;
using Infrastructure;

public class EnemyPrepareState : IEnterState, IExitState {
    private IStateMachine _stateMachine;
    private Game _game;

    public EnemyPrepareState(IStateMachine stateMachine, Game game) {
        _stateMachine = stateMachine;
        _game = game;
    }

    public void Enter() {
        _game.OnGameStart += OnGameStart;
    }

    public void Exit() {
        _game.OnGameStart -= OnGameStart;
    }

    private void OnGameStart() {
        _stateMachine.TranslateTo<EnemyIdleState>();
    }
}