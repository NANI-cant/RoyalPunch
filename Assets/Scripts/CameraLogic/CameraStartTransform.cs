using System;
using UnityEngine;

namespace CameraLogic {
    [Serializable]
    public class CameraStartTransform {
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _rotation;

        public Vector3 Position => _position;
        public Quaternion Rotation => Quaternion.Euler(_rotation);

    }
}