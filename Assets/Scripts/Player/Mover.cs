using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour, IMovable, IRotateable {
    [SerializeField] private float _linerSpeed;

    private CharacterController _controller;
    private Transform _transform;

    private Vector3 _gizmosDrawDirection;

    private void Awake() {
        _controller = GetComponent<CharacterController>();
        _transform = transform;
    }

    public void Move(Vector3 direction) {
        direction = _transform.TransformDirection(direction);
        _gizmosDrawDirection = direction;
        _controller.Move(direction * _linerSpeed * Time.deltaTime);
    }

    public void RotateTo(Vector3 point) {
        _transform.LookAt(point);
    }

    public void Disable() {
        enabled = false;
    }

    public void Enable() {
        enabled = true;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + _gizmosDrawDirection * 3f);
    }
}
