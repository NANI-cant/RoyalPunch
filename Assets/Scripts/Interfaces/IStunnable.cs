using UnityEngine;
using UnityEngine.Events;

public interface IStunnable {
    UnityAction OnStunStart { get; set; }
    UnityAction OnStunEnd { get; set; }
    void Stun();
}