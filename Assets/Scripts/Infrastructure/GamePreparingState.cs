using UI;

namespace Infrastructure {
    public class GamePreparingState : IEnterState, IExitState {
        private const bool ShouldLog = false;

        private GameStateMachine _stateMachine;
        private StartPanel _startPanel;
        private IInputService _inputService;
        private Game _game;

        public GamePreparingState(GameStateMachine stateMachine, Game game, StartPanel startPanel, IInputService inputService) {
            _stateMachine = stateMachine;
            _startPanel = startPanel;
            _inputService = inputService;
            _game = game;
        }

        private void TranslateToGameplay() {
            _stateMachine.TranslateTo<GameplayState>();
        }

        public void Enter() {
            this.Log(this + " Enter", ShouldLog);
            _startPanel.OnHitPlayButton += TranslateToGameplay;
            _inputService.Disable();
            _startPanel.gameObject.SetActive(true);
        }

        public void Exit() {
            this.Log(this + " Exit", ShouldLog);
            _startPanel.OnHitPlayButton -= TranslateToGameplay;
            _startPanel.gameObject.SetActive(false);
        }
    }
}
