using UnityEngine;

public abstract class SingleAttack : Attack {
    [Min(0f)]
    [SerializeField] protected float _delay = 0f;
    [Min(0f)]
    [SerializeField] protected float _damage;
}