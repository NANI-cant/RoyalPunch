using Infrastructure;

public class PlayerPrepareState : IEnterState, IExitState {
    private IStateMachine _stateMachine;
    private PlayerAvatar _avatar;
    private Game _game;

    public PlayerPrepareState(IStateMachine stateMachine, PlayerAvatar avatar, Game game) {
        _stateMachine = stateMachine;
        _avatar = avatar;
        _game = game;
    }

    public void Enter() {
        _avatar.PlayBattlePreparing();
        _game.OnGameStart += OnGameStart;
    }

    public void Exit() {
        _avatar.StopPreparing();
        _game.OnGameStart -= OnGameStart;
    }

    private void OnGameStart() {
        _stateMachine.TranslateTo<PlayerFreeState>();
    }
}