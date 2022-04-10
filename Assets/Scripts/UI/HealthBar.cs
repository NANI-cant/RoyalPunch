using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour {
    [SerializeField] private MonoBehaviour _damageableMB;

    private IDamageable _damageable;
    private Slider _slider;

#if UNITY_EDITOR
    private void OnValidate() {
        if (_damageableMB is IDamageable == false) {
            _damageableMB = null;
        }
        else {
            _damageable = (IDamageable)_damageableMB;
        }
    }
#endif

    private void Awake() {
        _slider = GetComponent<Slider>();
        _slider.value = 1f;
    }

    private void OnEnable() {
        _damageable.OnDamageTaked += OnDamageTaked;
    }

    private void OnDisable() {
        _damageable.OnDamageTaked -= OnDamageTaked;
    }

    private void OnDamageTaked(TakeDamageResult takeDamageResult) {
        float newSliderValue = takeDamageResult.RemindedHealth / takeDamageResult.MaxHealth;
        _slider.value = newSliderValue;
    }
}
