using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

enum EnemyState {
    Idle,
    CastSpell,
    Stun
}

[RequireComponent(typeof(IDamageable))]
[RequireComponent(typeof(IDeathable))]
[RequireComponent(typeof(IRotateable))]
[RequireComponent(typeof(IAttackable))]
[RequireComponent(typeof(EnemyAvatar))]
[RequireComponent(typeof(SpellHandler))]
[RequireComponent(typeof(IStunnable))]
public class Enemy : MonoBehaviour {
    private Transform _lookAtIt;

    private IDamageable _damageabler;
    private IDeathable _deathabler;
    private IRotateable _rotator;
    private IAttackable _attacker;
    private IStunnable _stunnabler;
    private EnemyAvatar _avatar;
    private SpellHandler _spellHandler;

    private EnemyState _currentState;
    private UnityAction _transitionToStun;
    private UnityAction _transitionToIdle;

    [Inject]
    private void Constructor(Player player) {
        _lookAtIt = player.transform;
        _damageabler = GetComponent<IDamageable>();
        _deathabler = GetComponent<IDeathable>();
        _rotator = GetComponent<IRotateable>();
        _attacker = GetComponent<IAttackable>();
        _stunnabler = GetComponent<IStunnable>();
        _avatar = GetComponent<EnemyAvatar>();
        _spellHandler = GetComponent<SpellHandler>();

        _transitionToStun = () => TranslateTo(EnemyState.Stun);
        _transitionToIdle = () => TranslateTo(EnemyState.Idle);
    }

    private void OnEnable() {
        _spellHandler.OnCastEnd += _stunnabler.Stun;
        _stunnabler.OnStunStart += _transitionToStun;
        _stunnabler.OnStunEnd += _transitionToIdle;
    }

    private void OnDisable() {
        _spellHandler.OnCastEnd -= _stunnabler.Stun;
        _stunnabler.OnStunStart -= _transitionToStun;
        _stunnabler.OnStunEnd -= _transitionToIdle;
    }

    private void Start() {
        EnterState(EnemyState.Idle);
    }

    private void FixedUpdate() {
        if (_currentState == EnemyState.Idle) {
            _rotator.RotateTo(_lookAtIt.position);

            if (_attacker.CouldAttack) {
                _attacker.Attack();
                _avatar.PlayAttack();
            }

            if (_spellHandler.CouldCast()) {
                _spellHandler.CastRandom();
                TranslateTo(EnemyState.CastSpell);
            }
        }
    }

    private void TranslateTo(EnemyState state) {
        ExitState(_currentState);
        _currentState = state;
        EnterState(_currentState);
    }

    private void EnterState(EnemyState state) {
        switch (state) {
            case EnemyState.Stun: {
                    _avatar.PlayStun();
                    break;
                }
            default: { break; }
        }
    }

    private void ExitState(EnemyState state) {

    }
}
