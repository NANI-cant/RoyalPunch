using System;
using UnityEngine.Events;

public class EnemyStunState : IEnterState, IExitState {
    private IStateMachine _stateMachine;
    private EnemyAvatar _avatar;
    private IStunnable _stunnable;

    public EnemyStunState(IStateMachine stateMachine, EnemyAvatar avatar, IStunnable stunnable) {
        _stateMachine = stateMachine;
        _avatar = avatar;
        _stunnable = stunnable;
    }

    public void Enter() {
        _stunnable.OnStunEnd += OnStunEnd;
        _avatar.PlayStun();
    }


    public void Exit() {
        _stunnable.OnStunEnd -= OnStunEnd;
    }

    private void OnStunEnd() {
        _stateMachine.TranslateTo<EnemyIdleState>();
    }
}