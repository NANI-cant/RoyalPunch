using System;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure {
    public class GamePreparingState : IEnterState, IExitState {
        private const bool ShouldLog = false;

        private GameStateMachine _stateMachine;

        public GamePreparingState(GameStateMachine stateMachine) {
            _stateMachine = stateMachine;
        }

        private void TranslateToGameplay() {
            _stateMachine.TranslateTo<GameplayState>();
        }

        public void Enter() {
            this.Log(this + " Enter", ShouldLog);
        }

        public void Exit() {
            this.Log(this + " Exit", ShouldLog);
        }
    }
}
