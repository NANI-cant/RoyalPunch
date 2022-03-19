namespace Infrastructure {
    public class GameplayState : IEnterState {
        private const bool ShouldLog = true;

        private GameStateMachine _gameStateMachine;
        private IInputService _inputService;

        public GameplayState(GameStateMachine gameStateMachine, IInputService inputService) {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
        }

        public void Enter() {
            this.Log(this + " Enter", ShouldLog);
            _inputService.Enable();
        }
    }
}
