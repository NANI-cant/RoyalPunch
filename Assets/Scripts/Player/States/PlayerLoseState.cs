public class PlayerLoseState : IState {
    private IStateMachine _stateMachine;

    public PlayerLoseState(IStateMachine stateMachine) {
        _stateMachine = stateMachine;
    }
}