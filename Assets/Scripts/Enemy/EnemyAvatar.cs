using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvatar : MonoBehaviour {
    [SerializeField] private Animator _animator;

    private const int GeneralLayerId = 0;
    private const float FromStart = 0f;
    private const string AutoPunchState = "AutoPunch";
    private const string BigStompState = "BigStomp";
    private const string StunState = "Stun";

    public void PlayAttack() {
        _animator.Play(AutoPunchState, GeneralLayerId, 0f);
    }

    public void PlayBigStomp() {
        _animator.Play(BigStompState, GeneralLayerId, FromStart);
    }

    public void PlayStun() {
        _animator.Play(StunState, GeneralLayerId, FromStart);
    }
}
