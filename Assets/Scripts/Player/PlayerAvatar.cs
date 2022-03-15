using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _spine;
    [SerializeField] private Ragdoll _ragdoll;

    private const string AutoPunchState = "AutoPunch";
    private const string ForwardRunState = "ForwardRun";
    private const string BackwardRunState = "BackwardRun";

    private int lowerBodyId => _animator.GetLayerIndex("LowerBody");
    private int upperBodyId => _animator.GetLayerIndex("UpperBody");
    private int generalId => _animator.GetLayerIndex("General");

    public void PlayAutoPunch() {
        EnableLayer(upperBodyId);
        _animator.Play(AutoPunchState, upperBodyId, 0f);
    }

    public void PlayMoveInDirection(Vector2 direction) {
        if (direction == Vector2.zero) {
            DisableLayer(lowerBodyId);
            return;
        }

        EnableLayer(lowerBodyId);
        if (direction.y >= 0) {
            _animator.Play(ForwardRunState, lowerBodyId);
        }
        else {
            _animator.Play(BackwardRunState, lowerBodyId);
        }
        DirectModelTo(direction.To3Dimentions());
    }

    public void FallDown() {
        _animator.enabled = false;
        _ragdoll.SaveBones();
        _ragdoll.Enable();
    }

    public void StandUp() {
        _ragdoll.Disable();
        _ragdoll.LoadBones();
        _animator.enabled = true;
    }

    private void DirectModelTo(Vector3 direction) {
        //direction = direction == Vector3.zero ? Vector3.forward : direction;
        Transform animatorTransform = _animator.transform;
        Quaternion targetQuaternion = Quaternion.LookRotation(direction.z * transform.TransformDirection(direction), Vector3.up);
        const float DeltaQuaternion = 0.1f;
        animatorTransform.rotation = Quaternion.Lerp(animatorTransform.rotation, targetQuaternion, DeltaQuaternion);
    }

    private void EnableLayer(int id) {
        _animator.SetLayerWeight(id, 1);
    }

    private void DisableLayer(int id) {
        _animator.SetLayerWeight(id, 0);
    }
}
