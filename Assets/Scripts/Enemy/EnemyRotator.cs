using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotator : MonoBehaviour, IRotateable {
    [SerializeField] private float _angularSpeed;

    private Transform _transform;

    private void Awake() {
        _transform = transform;
    }

    public void RotateTo(Vector3 point) {
        Vector3 directionToPoint = (point - _transform.position).normalized;
        Quaternion targetQuaternion = Quaternion.LookRotation(directionToPoint, Vector3.up);
        _transform.rotation = Quaternion.Lerp(_transform.rotation, targetQuaternion, _angularSpeed * Mathf.Deg2Rad * Time.deltaTime);
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
