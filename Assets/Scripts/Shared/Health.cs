using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable, IDeathable {
    [Min(0)]
    [SerializeField] private float _maxHealth = 1f;

    public UnityAction<TakeDamageResult> OnDamageTaked { get; set; }
    public UnityAction OnDeath { get; set; }

    private float _currentHealth;

    private void Start() {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage) {
        if (_currentHealth < damage) return;

        Debug.Log(this + " Took " + damage + " damage, now " + _currentHealth + "/" + _maxHealth + " health");
        _currentHealth -= damage;
        OnDamageTaked?.Invoke(new TakeDamageResult(_maxHealth, _currentHealth, damage));
        if (_currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        OnDeath?.Invoke();
    }
}
