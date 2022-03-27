public class EnemyWinState : IState {
    private IStateMachine _stateMachine;

    public EnemyWinState(IStateMachine stateMachine) {
        _stateMachine = stateMachine;
    }
}