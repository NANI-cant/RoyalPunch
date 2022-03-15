using UnityEngine;
using UnityEngine.Events;

public interface IDamageable {
    UnityAction<TakeDamageResult> OnDamageTaked { get; set; }
    void TakeDamage(float damage);
}
