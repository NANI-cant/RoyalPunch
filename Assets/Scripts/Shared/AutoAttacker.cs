using System.Collections.Generic;
using UnityEngine;

public class AutoAttacker : MonoBehaviour, IAttackable {
    [SerializeField] private float _damage;
    [SerializeField] private float _timeBetweenAttacks = 0.5f;
    [Min(0f)]
    [SerializeField] private float _delay = 0f;
    [SerializeField] private LayerMask _enemieLayer;
    [SerializeField] private Vector3 _attackCenter = Vector3.zero;
    [SerializeField] private Vector3 _attackSize = Vector3.one;
    [Header("Debug")]
    [SerializeField] private bool _shouldLog = false;

    private float _savedAttackTime = float.NegativeInfinity;
    private bool _debugShouldFlush = false;

    public bool CouldAttack => GetDamageables().Count > 0 && hasEnoughTimePassedBetweenAttacks;

    private bool hasEnoughTimePassedBetweenAttacks => (Time.time - _savedAttackTime) >= _timeBetweenAttacks;

#if UNITY_EDITOR
    private void OnValidate() {
        if (_delay > _timeBetweenAttacks) _delay = _timeBetweenAttacks;
    }
#endif

    public void Attack() {
        if (hasEnoughTimePassedBetweenAttacks == false) return;
        List<IDamageable> damageables = GetDamageables();
        if (damageables.Count <= 0) return;
        _savedAttackTime = Time.time;
        this.Invoke(() => {
            DoDamageTo(damageables);
            _debugShouldFlush = true;
        }, _delay);
        this.Invoke(() => _debugShouldFlush = false, _delay + (_timeBetweenAttacks - _delay) / 3);
    }

    public void Disable() {
        enabled = false;
    }

    public void Enable() {
        enabled = true;
    }

    private void DoDamageTo(List<IDamageable> damageables) {
        this.Log("Bang", when: _shouldLog);
        foreach (var damageable in damageables) {
            damageable.TakeDamage(_damage);
        }
    }

    private List<IDamageable> GetDamageables() {
        List<IDamageable> damageables = new List<IDamageable>();

        Collider[] enemiesColliders = Physics.OverlapBox(
                transform.TransformPoint(_attackCenter),
                _attackSize,
                transform.rotation,
                _enemieLayer,
                QueryTriggerInteraction.Ignore
            );
        if (enemiesColliders.Length <= 0) {
            return damageables;
        }

        foreach (var collider in enemiesColliders) {
            if (collider.TryGetComponent<IDamageable>(out IDamageable damageable)) {
                damageables.Add(damageable);
            }
        }
        return damageables;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        if (_debugShouldFlush) {
            Gizmos.DrawCube(_attackCenter, _attackSize);
        }
        else {
            Gizmos.DrawWireCube(_attackCenter, _attackSize);
        }
    }
}
