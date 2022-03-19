using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Camera))]
public class CameraFollowing : MonoBehaviour {
    [Header("Looking")]
    [SerializeField] private Transform _lookAt;
    [SerializeField] private float _angleOffset;
    [Header("Following")]
    [SerializeField] private Transform _followIt;
    [SerializeField] private Vector3 _followOffset;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool _isValidate;

    private void OnValidate() {
        if (!_isValidate) return;
        
        if (_lookAt != null)
            RotateTo(_lookAt.position);
        if (_followIt != null)
            MoveTo(_followIt.position);
    }
#endif

    [Inject]
    private void Constructor(Player player, Enemy enemy) {
        _followIt = player.transform;
        _lookAt = enemy.transform;
    }

    private void Update() {
        RotateTo(_lookAt.position);
    }

    private void FixedUpdate() {
        MoveTo(_followIt.position);
    }

    private void RotateTo(Vector3 point) {
        transform.LookAt(point);
        transform.Rotate(new Vector3(-_angleOffset, 0f, 0f));
    }

    private void MoveTo(Vector3 point) {
        transform.position = point + _followIt.transform.rotation * _followOffset;
    }
}
