using System;
using System.Collections.Generic;
using UI;

namespace Infrastructure {
    public class GameStateMachine : IStateMachine {
        private readonly Dictionary<Type, IState> _states;
        private IState _activeState;

        public GameStateMachine(
            Game game,
            Player player,
            Enemy enemy,
            StartPanel startPanel,
            EndGamePanel endGamePanel,
            IInputService inputService) {
            _states = new Dictionary<Type, IState> {
                [typeof(GamePreparingState)] = new GamePreparingState(this, game, startPanel, endGamePanel, inputService),
                [typeof(GameplayState)] = new GameplayState(this, game, player, enemy, inputService),
                [typeof(GameLoseState)] = new GameLoseState(this, game, inputService, endGamePanel),
                [typeof(GameWinState)] = new GameWinState(this, game, inputService, endGamePanel),
            };
        }

        public void TranslateTo<TState>() where TState : IState {
            _activeState?.Do(() => ((IExitState)_activeState).Exit(), when: _activeState is IExitState);
            _activeState = _states[typeof(TState)];
            _activeState.Do(() => ((IEnterState)_activeState).Enter(), when: _activeState is IEnterState);
        }
    }
}
