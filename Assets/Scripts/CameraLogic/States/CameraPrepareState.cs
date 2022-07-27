using Infrastructure;
using UnityEngine;

namespace CameraLogic {
    public class CameraPrepareState : IEnterState, IExitState {
        private IStateMachine _stateMachine;
        private Transform _cameraTransform;
        private Game _game;
        private CameraStartTransform _startTransform;

        public CameraPrepareState(
            IStateMachine stateMachine,
            Transform cameraTransform,
            Game game,
            CameraStartTransform startTransform) {
            _stateMachine = stateMachine;
            _cameraTransform = cameraTransform;
            _game = game;
            _startTransform = startTransform;
        }

        public void Enter() {
            _cameraTransform.position = _startTransform.Position;
            _cameraTransform.rotation = _startTransform.Rotation;
            _game.OnGameStart += OnGameStart;
        }


        public void Exit() {
            _game.OnGameStart -= OnGameStart;
        }

        private void OnGameStart() {
            _stateMachine.TranslateTo<CameraFollowState>();
        }
    }
}