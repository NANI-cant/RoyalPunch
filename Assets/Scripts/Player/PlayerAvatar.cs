using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

public class PlayerAvatar : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private float _fallingDuration;
    [SerializeField] private float _standingDuration;
    [SerializeField] private Ragdoll _ragdoll;
    [SerializeField] private RigBuilder _rigBuilder;
    [SerializeField] private MultiAimConstraint _torsoAim;

    private const string AutoPunchState = "AutoPunch";
    private const string ForwardRunState = "ForwardRun";
    private const string BackwardRunState = "BackwardRun";

    private int lowerBodyId => _animator.GetLayerIndex("LowerBody");
    private int upperBodyId => _animator.GetLayerIndex("UpperBody");
    private int generalId => _animator.GetLayerIndex("General");

    [Inject]
    private void Constructor(Enemy enemy) {
        var sourceObjects = _torsoAim.data.sourceObjects;
        sourceObjects.SetTransform(0, enemy.transform);
        _torsoAim.data.sourceObjects = sourceObjects;
        _rigBuilder.Build();
    }

    public void PlayAutoPunch() {
        EnableLayer(upperBodyId);
        _animator.Play(AutoPunchState, upperBodyId, 0f);
    }

    public void PlayMoveInDirection(Vector2 direction) {
        if (direction == Vector2.zero) {
            DirectModelTo(Vector3.forward);
            DisableLayer(lowerBodyId);
            return;
        }

        EnableLayer(lowerBodyId);
        DirectModelTo(direction.To3Dimentions());
        if (direction.y >= 0) {
            _animator.Play(ForwardRunState, lowerBodyId);
        }
        else {
            _animator.Play(BackwardRunState, lowerBodyId);
        }
    }

    public void FallDown() {
        _animator.enabled = false;
        _ragdoll.SaveBones();
        _ragdoll.Enable();
        this.Invoke(() => StandUp(), _fallingDuration);
    }

    public void StandUp() {
        _ragdoll.Disable();
        _ragdoll.LoadBonesSmooth(_standingDuration);
        _animator.enabled = true;
    }

    private void DirectModelTo(Vector3 direction) {
        Transform animatorTransform = _animator.transform;
        Quaternion targetQuaternion = Quaternion.LookRotation(direction.z * transform.TransformDirection(direction), Vector3.up);
        const float DeltaQuaternion = 0.1f;
        animatorTransform.rotation = Quaternion.Lerp(animatorTransform.rotation, targetQuaternion, DeltaQuaternion);
    }

    private void EnableLayer(int id) {
        _animator.SetLayerWeight(id, 1);
    }

    private void DisableLayer(int id) {
        _animator.SetLayerWeight(id, 0);
    }
}
