using UnityEngine;

public class AutoAttacker : MonoBehaviour, IAttackable {
    [SerializeField] MultipleMeleeAttack _attack;
    [Header("Debug")]
    [SerializeField] private bool _shouldLog = false;

    private Coroutine _attackCoroutine;

    public bool CouldAttack => _attack.CouldAttack(this);

#if UNITY_EDITOR
    private void OnValidate() {
        _attack.Validate(this);
    }
#endif

    public void Attack() {
        _attackCoroutine = _attack.Perform(this)[0];
    }

    public void Disable() {
        enabled = false;
    }

    public void Enable() {
        enabled = true;
    }

    private void OnDrawGizmos() {
        _attack.DrawGizmos(this);
    }
}
