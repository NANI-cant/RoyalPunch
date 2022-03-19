using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure {
    public class GameStateMachine : IStateMachine {
        private readonly Dictionary<Type, IState> _states;
        private IState _activeState;

        public GameStateMachine() {
            _states = new Dictionary<Type, IState> {
                [typeof(GamePreparingState)] = new GamePreparingState(this),
            };
        }

        public void TranslateTo<TState>() where TState : IState {
            _activeState?.Do(() => ((IExitState)_activeState).Exit(), when: _activeState is IExitState);
            _activeState = _states[typeof(TState)];
            _activeState.Do(() => ((IEnterState)_activeState).Enter(), when: _activeState is IEnterState);
        }
    }
}
