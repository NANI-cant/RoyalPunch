using System.Collections.Generic;
using UnityEngine;

public abstract class Attack {
    [Min(0f)]
    [SerializeField] protected float _recoverTime = 0f;

    public abstract List<Coroutine> Perform(MonoBehaviour attacker);
    public abstract bool CouldAttack(MonoBehaviour attacker);
    public virtual void DrawGizmos(MonoBehaviour attacker) { }
    public virtual void Validate(MonoBehaviour attacker) { }
}
