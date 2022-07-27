public class PlayerWinState : IEnterState {
    private IStateMachine _stateMachine;
    private PlayerAvatar _avatar;

    public PlayerWinState(IStateMachine stateMachine, PlayerAvatar avatar) {
        _stateMachine = stateMachine;
        _avatar = avatar;
    }

    public void Enter() {
        _avatar.PlayWinDance();
    }
}