using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable, IDeathable {
    [Min(0)]
    [SerializeField] private float _maxHealth = 1f;
    [Header("Debug")]
    [SerializeField] private bool _shouldLog = false;

    public UnityAction<TakeDamageResult> OnDamageTaked { get; set; }
    public UnityAction OnDeath { get; set; }

    private float _currentHealth;

    private void Start() {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage) {
        if (_currentHealth < damage) return;

        this.Log(this + " Took " + damage + " damage, now " + _currentHealth + "/" + _maxHealth + " health", when: _shouldLog);
        _currentHealth -= damage;
        OnDamageTaked?.Invoke(new TakeDamageResult(_maxHealth, _currentHealth, damage));
        if (_currentHealth <= 0) {
            Die();
        }
    }

    public void Disable() {
        enabled = false;
    }

    public void Enable() {
        enabled = true;
    }

    private void Die() {
        OnDeath?.Invoke();
    }
}
