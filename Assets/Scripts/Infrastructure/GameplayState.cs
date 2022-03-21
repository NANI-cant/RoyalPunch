namespace Infrastructure {
    public class GameplayState : IEnterState, IExitState {
        private const bool ShouldLog = true;

        private GameStateMachine _stateMachine;
        private IInputService _inputService;
        private Game _game;
        private Player _player;
        private Enemy _enemy;

        public GameplayState(GameStateMachine gameStateMachine, Game game, Player player, Enemy enemy, IInputService inputService) {
            _stateMachine = gameStateMachine;
            _inputService = inputService;
            _game = game;
            _player = player;
            _enemy = enemy;
        }

        public void Enter() {
            this.Log(this + " Enter", ShouldLog);
            _inputService.Enable();
            _player.GetComponent<IDeathable>().OnDeath += OnPlayerDeath;
            _enemy.GetComponent<IDeathable>().OnDeath += OnEnemyDeath;
            _game.OnGameStart?.Invoke();
        }


        public void Exit() {
            this.Log(this + " Exit", ShouldLog);
            _player.GetComponent<IDeathable>().OnDeath -= OnPlayerDeath;
            _enemy.GetComponent<IDeathable>().OnDeath -= OnEnemyDeath;
        }

        private void OnPlayerDeath() {
            _stateMachine.TranslateTo<GameLoseState>();
        }

        private void OnEnemyDeath() {
            _stateMachine.TranslateTo<GameWinState>();
        }
    }
}
