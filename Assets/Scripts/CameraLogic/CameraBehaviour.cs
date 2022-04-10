using System;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using Zenject;

namespace CameraLogic {
    public class CameraBehaviour : MonoBehaviour, IStateMachine {
        [SerializeField] private CameraStartTransform _startTransform;
        [SerializeField] private CameraFollowing _following;

        [Header("Debug")]
        [SerializeField] private bool _shouldLog;
        [SerializeField] private DebugState _debugState;
        enum DebugState {
            Preparing,
            Following
        }

        private Game _game;
        private IState _activeState;
        private Dictionary<Type, IState> _states;

#if UNITY_EDITOR
        [ContextMenu("ReValidate")]
        private void OnValidate() {
            switch (_debugState) {
                case DebugState.Preparing: {
                        transform.position = _startTransform.Position;
                        transform.rotation = _startTransform.Rotation;
                        break;
                    }
                case DebugState.Following: {
                        _following.Move();
                        _following.Rotate();
                        break;
                    }
            }
        }
#endif

        [Inject]
        private void Constructor(Game game) {
            _game = game;
            _states = new Dictionary<Type, IState> {
                [typeof(CameraPrepareState)] = new CameraPrepareState(this, transform, _game, _startTransform),
                [typeof(CameraFollowState)] = new CameraFollowState(this, _game, _following),
            };
            TranslateTo<CameraPrepareState>();
        }

        private void Update() {
            _activeState?.Do(
                () => ((IUpdateState)_activeState).Update(),
                when: _activeState is IUpdateState
            );
        }

        private void FixedUpdate() {
            _activeState?.Do(
                () => ((IFixedUpdateState)_activeState).FixedUpdate(),
                when: _activeState is IFixedUpdateState
            );
        }

        public void TranslateTo<TState>() where TState : IState {
            _activeState?.Do(
                () => {
                    ((IExitState)_activeState).Exit();
                    this.Log("Exit " + _activeState, _shouldLog);
                },
                when: _activeState is IExitState
            );
            _activeState = _states[typeof(TState)];
            _activeState.Do(
                () => {
                    ((IEnterState)_activeState).Enter();
                    this.Log("Enter " + _activeState, _shouldLog);
                },
                when: _activeState is IEnterState
            );
        }
    }
}
