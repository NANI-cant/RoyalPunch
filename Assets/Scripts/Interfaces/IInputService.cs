using UnityEngine;

public interface IInputService : IEnable, IDisable {
    Vector2 Direction { get; }
}
