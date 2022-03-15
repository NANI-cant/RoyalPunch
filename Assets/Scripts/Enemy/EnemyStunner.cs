using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStunner : MonoBehaviour, IStunnable {
    [SerializeField] private float _duration;

    public UnityAction OnStunStart { get; set; }
    public UnityAction OnStunEnd { get; set; }

    public void Stun() {
        OnStunStart?.Invoke();
        this.Invoke(() => {
            OnStunEnd?.Invoke();
        }, _duration);
    }
}
