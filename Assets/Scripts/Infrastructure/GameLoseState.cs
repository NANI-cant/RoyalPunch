namespace Infrastructure {
    public class GameLoseState : IEnterState {
        private const bool ShouldLog = true;

        private GameStateMachine _stateMachine;
        private Game _game;
        private IInputService _inputService;

        public GameLoseState(GameStateMachine gameStateMachine, Game game, IInputService inputService) {
            _stateMachine = gameStateMachine;
            _game = game;
            _inputService = inputService;
        }

        public void Enter() {
            this.Log(this + " Enter", when: ShouldLog);
            _inputService.Disable();
            _game.OnGameLoose?.Invoke();
        }
    }
}
