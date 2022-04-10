namespace Infrastructure {
    public class GameLoseState : IEnterState {
        private const bool ShouldLog = true;

        private GameStateMachine _stateMachine;
        private Game _game;
        private IInputService _inputService;
        private EndGamePanel _endGamePanel;

        public GameLoseState(GameStateMachine gameStateMachine, Game game, IInputService inputService, EndGamePanel endGamePanel) {
            _stateMachine = gameStateMachine;
            _game = game;
            _inputService = inputService;
            _endGamePanel = endGamePanel;
        }

        public void Enter() {
            this.Log(this + " Enter", when: ShouldLog);
            _inputService.Disable();
            _game.OnGameLoose?.Invoke();
            _endGamePanel.gameObject.SetActive(true);
        }
    }
}
