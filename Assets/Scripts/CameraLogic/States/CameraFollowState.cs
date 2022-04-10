using Infrastructure;

namespace CameraLogic {
    public class CameraFollowState : IEnterState, IExitState, IUpdateState, IFixedUpdateState {
        private IStateMachine _stateMachine;
        private Game _game;
        private CameraFollowing _following;
        private SmoothTranslate _smoothTranslate;
        private bool _canFollow;

        public CameraFollowState(IStateMachine stateMachine, Game game, CameraFollowing following) {
            _stateMachine = stateMachine;
            _game = game;
            _following = following;
            _smoothTranslate = new SmoothTranslate(following);
        }

        public void Enter() {
            _canFollow = false;
            _smoothTranslate.Translated += OnTranslated;
            _smoothTranslate.Translate(_following.transform, 0.5f, _following.transform.position, _following.CalculatedPosition, _following.transform.rotation, _following.CalculatedRotation);
        }

        public void Exit() {
            _smoothTranslate.Translate(_following.transform, float.Epsilon, _following.transform.position, _following.CalculatedPosition, _following.transform.rotation, _following.CalculatedRotation);
            _smoothTranslate.Translated -= OnTranslated;
            _canFollow = false;
        }

        public void FixedUpdate() {
            if (!_canFollow) return;
            _following.Move();
        }

        public void Update() {
            if (!_canFollow) return;
            _following.Rotate();
        }

        private void OnTranslated() {
            _canFollow = true;
        }
    }
}