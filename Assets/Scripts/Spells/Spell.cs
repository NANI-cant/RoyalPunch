using UnityEngine;
using UnityEngine.Events;

public abstract class Spell : ScriptableObject {
    [SerializeField] protected bool _shouldDrawGizmos = false;
    [Space]
    [Min(0)]
    [SerializeField] protected float _damage;
    [SerializeField] protected LayerMask _whatIsDamageable;
    [Min(0)]
    [SerializeField] protected float _damageDelay;
    [Min(0)]
    [SerializeField] protected float _duration;

#if UNITY_EDITOR
    private void OnValidate() {
        if (_damageDelay > _duration) _damageDelay = _duration;
    }
#endif

    public abstract UnityAction OnPerformed { get; set; }

    public abstract bool CheckCouldPerform(MonoBehaviour performer);
    public abstract void Perform(MonoBehaviour performer);
    public virtual void DrawGizmos(MonoBehaviour performer) { }
}
