public class TakeDamageResult {
    private float _maxHealth;
    private float _remindedHealth;
    private float _takedDamage;

    public float MaxHealth => _maxHealth;
    public float RemindedHealth => _remindedHealth;
    public float TakedDamage => _takedDamage;

    public TakeDamageResult(float maxHealth, float remindedHealth, float takedDamage) {
        _maxHealth = maxHealth;
        _remindedHealth = remindedHealth;
        _takedDamage = takedDamage;
    }
}
