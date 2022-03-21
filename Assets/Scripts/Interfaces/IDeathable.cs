using System;
using UnityEngine.Events;

public interface IDeathable : IDisable, IEnable {
    UnityAction OnDeath { get; set; }
}
