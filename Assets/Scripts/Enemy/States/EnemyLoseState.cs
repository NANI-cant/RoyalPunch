public class EnemyLoseState : IEnterState {
    private IStateMachine _stateMachine;
    private EnemyAvatar _avatar;

    public EnemyLoseState(IStateMachine stateMachine, EnemyAvatar avatar) {
        _stateMachine = stateMachine;
        _avatar = avatar;
    }

    public void Enter() {
        _avatar.PlayLose();
    }
}