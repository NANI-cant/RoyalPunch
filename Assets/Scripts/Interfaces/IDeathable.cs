using UnityEngine.Events;

public interface IDeathable {
    UnityAction OnDeath { get; set; }
}
