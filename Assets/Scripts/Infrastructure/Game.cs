using UI;
using UnityEngine.Events;

namespace Infrastructure {
    public class Game {
        public UnityAction OnGameStart;
        public UnityAction OnGameLoose;
        public UnityAction OnGameWin;

        private GameStateMachine _stateMachine;

        public Game(Player player, Enemy enemy, StartPanel startPanel, IInputService inputService) {
            _stateMachine = new GameStateMachine(this, player, enemy, startPanel, inputService);
            _stateMachine.TranslateTo<GamePreparingState>();
        }
    }
}
