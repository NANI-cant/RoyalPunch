using UnityEngine;

public interface IMovable : IEnable, IDisable {
    void Move(Vector3 direction);
}
