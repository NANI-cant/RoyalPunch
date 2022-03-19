using UnityEngine;

public interface IInputService {
    Vector2 Direction { get; }
    void Disable();
    void Enable();
}
