using UnityEngine;
using UnityEngine.Events;

public interface IDamageable : IDisable, IEnable {
    UnityAction<TakeDamageResult> OnDamageTaked { get; set; }
    void TakeDamage(float damage);
}
