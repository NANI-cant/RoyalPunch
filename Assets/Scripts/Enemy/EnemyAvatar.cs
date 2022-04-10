using UnityEngine;

public class EnemyAvatar : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private Ragdoll _ragdoll;
    [SerializeField] private float _fallingDuration;
    [SerializeField] private float _standingDuration;

    private const int GeneralLayerId = 0;
    private const float FromStart = 0f;
    private const string AutoPunchState = "AutoPunch";
    private const string BigStompState = "BigStomp";
    private const string StunState = "Stun";
    private const string LoseState = "Lose";

    public void PlayAttack() {
        _animator.Play(AutoPunchState, GeneralLayerId, 0f);
    }

    public void PlayBigStomp() {
        _animator.Play(BigStompState, GeneralLayerId, FromStart);
    }

    public void PlayStun() {
        _animator.Play(StunState, GeneralLayerId, FromStart);
    }

    public void PlayLose() {
        FallDown();
        this.Invoke(() => _animator.Play(LoseState, GeneralLayerId), _standingDuration + _fallingDuration);
    }

    public void FallDown() {
        _animator.enabled = false;
        _ragdoll.SaveBones();
        _ragdoll.Enable();
        this.Invoke(() => StandUp(_standingDuration), _fallingDuration);
    }

    public void StandUp(float duration = 0) {
        _ragdoll.Disable();
        _ragdoll.LoadBonesSmooth(duration);
        this.Invoke(() => _animator.enabled = true, duration);
    }
}
