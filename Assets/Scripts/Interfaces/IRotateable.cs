using UnityEngine;

public interface IRotateable : IEnable, IDisable {
    void RotateTo(Vector3 point);
}
