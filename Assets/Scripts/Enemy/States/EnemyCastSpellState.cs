public class EnemyCastSpellState : IEnterState, IExitState {
    private IStateMachine _stateMachine;
    private SpellHandler _spellHandler;
    private IStunnable _stunnable;

    public EnemyCastSpellState(IStateMachine stateMachine, SpellHandler spellHandler, IStunnable stunnable) {
        _stateMachine = stateMachine;
        _spellHandler = spellHandler;
        _stunnable = stunnable;
    }

    public void Enter() {
        _spellHandler.OnCastEnd += OnCastEnd;
    }

    public void Exit() {
        _spellHandler.OnCastEnd -= OnCastEnd;
    }

    private void OnCastEnd() {
        _stunnable.Stun();
        _stateMachine.TranslateTo<EnemyStunState>();
    }
}