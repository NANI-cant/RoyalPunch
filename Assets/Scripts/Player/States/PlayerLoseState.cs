public class PlayerLoseState : IEnterState {
    private IStateMachine _stateMachine;
    private PlayerAvatar _avatar;

    public PlayerLoseState(IStateMachine stateMachine, PlayerAvatar avatar) {
        _stateMachine = stateMachine;
        _avatar = avatar;
    }

    public void Enter() {
        _avatar.PlayLose();
    }
}