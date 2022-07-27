namespace Infrastructure {
    public class GameWinState : IEnterState {
        private const bool ShouldLog = true;

        private GameStateMachine _stateMachine;
        private Game _game;
        private IInputService _inputService;
        private EndGamePanel _endGamePanel;

        public GameWinState(GameStateMachine gameStateMachine, Game game, IInputService inputService, EndGamePanel endGamePanel) {
            _stateMachine = gameStateMachine;
            _game = game;
            _inputService = inputService;
            _endGamePanel = endGamePanel;
        }

        public void Enter() {
            this.Log(this + " Enter", when: ShouldLog);
            _inputService.Disable();
            _game.OnGameWin?.Invoke();
            _endGamePanel.gameObject.SetActive(true);
        }
    }
}
