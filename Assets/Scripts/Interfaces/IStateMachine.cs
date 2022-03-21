public interface IStateMachine {
    void TranslateTo<TState>() where TState : IState;
}
