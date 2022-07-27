using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeleeAttack : SingleAttack {
    [SerializeField] private LayerMask _enemieLayer;
    [SerializeField] private Vector3 _attackCenter = Vector3.zero;
    [SerializeField] private Vector3 _attackSize = Vector3.one;

    private float _savedAttackTime = float.NegativeInfinity;
    private bool _debugShouldFlush = false;

    private bool hasEnoughTimePassedBetweenAttacks => (Time.time - _savedAttackTime) >= _recoverTime;

    public override void Validate(MonoBehaviour attacker) {
        base.Validate(attacker);
        if (_delay > _recoverTime) _delay = _recoverTime;
    }

    public override List<Coroutine> Perform(MonoBehaviour attacker) {
        List<Coroutine> attackCoroutines = new List<Coroutine>();
        if (hasEnoughTimePassedBetweenAttacks == false) return attackCoroutines;

        List<IDamageable> damageables = GetDamageables(attacker);
        if (damageables.Count <= 0) return attackCoroutines;

        _savedAttackTime = Time.time;
        Coroutine attackCoroutine = attacker.Invoke(() => {
            DoDamageTo(damageables);
            _debugShouldFlush = true;
        }, _delay);
        attacker.Invoke(() => _debugShouldFlush = false, _delay + (_recoverTime - _delay) / 3);
        attackCoroutines.Add(attackCoroutine);
        return attackCoroutines;
    }

    public override void DrawGizmos(MonoBehaviour attacker) {
        base.DrawGizmos(attacker);
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(attacker.transform.position, attacker.transform.rotation, attacker.transform.lossyScale);
        if (_debugShouldFlush) {
            Gizmos.DrawCube(_attackCenter, _attackSize);
        }
        else {
            Gizmos.DrawWireCube(_attackCenter, _attackSize);
        }
    }

    public override bool CouldAttack(MonoBehaviour attacker) =>
        GetDamageables(attacker).Count > 0 && hasEnoughTimePassedBetweenAttacks;

    private void DoDamageTo(List<IDamageable> damageables) {
        foreach (var damageable in damageables) {
            damageable.TakeDamage(_damage);
        }
    }

    private List<IDamageable> GetDamageables(MonoBehaviour attacker) {
        List<IDamageable> damageables = new List<IDamageable>();

        Collider[] enemiesColliders = Physics.OverlapBox(
                attacker.transform.TransformPoint(_attackCenter),
                _attackSize,
                attacker.transform.rotation,
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
}