using UnityEngine;
using UnityEngine.Events;

public interface IStunnable : IEnable, IDisable {
    UnityAction OnStunStart { get; set; }
    UnityAction OnStunEnd { get; set; }
    void Stun();
}