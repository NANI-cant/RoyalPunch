using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure {
    public class Game {
        private GameStateMachine _stateMachine;

        public Game() {
            _stateMachine = new GameStateMachine();

            _stateMachine.TranslateTo<GamePreparingState>();
        }
    }
}
