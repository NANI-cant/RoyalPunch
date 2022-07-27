using System;

public class PlayerStunState : IEnterState, IExitState {
    private IStateMachine _stateMachine;
    private PlayerAvatar _avatar;
    private IStunnable _stunnable;

    public PlayerStunState(IStateMachine stateMachine, PlayerAvatar avatar, IStunnable stunnable) {
        _stateMachine = stateMachine;
        _avatar = avatar;
        _stunnable = stunnable;
    }

    public void Enter() {
        _avatar.FallDown();
        _stunnable.OnStunEnd += OnStunEnd;
    }

    public void Exit() {
        _avatar.StandUp();
        _stunnable.OnStunEnd -= OnStunEnd;
    }

    private void OnStunEnd() {
        _stateMachine.TranslateTo<PlayerFreeState>();
    }
}