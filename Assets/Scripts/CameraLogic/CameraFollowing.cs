using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CameraLogic {
    [RequireComponent(typeof(Camera))]
    public class CameraFollowing : MonoBehaviour {
        [Header("Looking")]
        [SerializeField] private Transform _lookAt;
        [SerializeField] private float _angleOffset;
        [Header("Following")]
        [SerializeField] private Transform _followIt;
        [SerializeField] private Vector3 _followOffset;

        public Vector3 CalculatedPosition => _followIt.position + _followIt.transform.rotation * _followOffset;
        public Quaternion CalculatedRotation => Quaternion.LookRotation((_lookAt.transform.position - CalculatedPosition).normalized, Vector3.up) * Quaternion.Euler(new Vector3(-_angleOffset, 0f, 0f));

        [Inject]
        private void Constructor(Player player, Enemy enemy) {
            _followIt = player.transform;
            _lookAt = enemy.transform;
        }

        public void Rotate() {
            transform.LookAt(_lookAt.position);
            transform.Rotate(new Vector3(-_angleOffset, 0f, 0f));
        }

        public void Move() {
            transform.position = _followIt.position + _followIt.transform.rotation * _followOffset;
        }
    }
}
