using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

enum PlayerState {
    Free,
    Stunned,
    Death,
}

[RequireComponent(typeof(IMovable))]
[RequireComponent(typeof(IRotateable))]
[RequireComponent(typeof(IAttackable))]
[RequireComponent(typeof(IDamageable))]
[RequireComponent(typeof(IDeathable))]
[RequireComponent(typeof(IStunnable))]
[RequireComponent(typeof(PlayerAvatar))]
public class Player : MonoBehaviour {
    [SerializeField] private bool _shouldLog;

    private Transform _lookAtThis;
    private PlayerState _currentState;

    private IInputService _input;
    private IMovable _mover;
    private IRotateable _rotator;
    private IAttackable _attacker;
    private IDamageable _damageabler;
    private IDeathable _deathabler;
    private IStunnable _stunnabler;
    private PlayerAvatar _avatar;

    private UnityAction _translationToFree;
    private UnityAction _translationToDeath;
    private UnityAction _translationToStunned;

    [Inject]
    private void Constructor(IInputService input, Enemy enemy) {
        _input = input;
        _lookAtThis = enemy.transform;

        _mover = GetComponent<IMovable>();
        _rotator = GetComponent<IRotateable>();
        _attacker = GetComponent<IAttackable>();
        _damageabler = GetComponent<IDamageable>();
        _deathabler = GetComponent<IDeathable>();
        _stunnabler = GetComponent<IStunnable>();
        _avatar = GetComponent<PlayerAvatar>();

        _translationToFree = () => TranslateTo(PlayerState.Free);
        _translationToDeath = () => TranslateTo(PlayerState.Death);
        _translationToStunned = () => TranslateTo(PlayerState.Stunned);
    }

    private void OnEnable() {
        _stunnabler.OnStunStart += _translationToStunned;
        _stunnabler.OnStunEnd += _translationToFree;
        _deathabler.OnDeath += OnDeath;
    }

    private void OnDisable() {
        _stunnabler.OnStunStart -= _translationToStunned;
        _stunnabler.OnStunEnd -= _translationToFree;
        _deathabler.OnDeath -= OnDeath;
    }

    private void Update() {
        if (_currentState == PlayerState.Free) {
            _avatar.PlayMoveInDirection(_input.Direction);
        }
    }

    private void FixedUpdate() {
        if (_currentState == PlayerState.Free) {
            _mover.Move(_input.Direction.To3Dimentions());
            _rotator.RotateTo(_lookAtThis.position);

            if (_attacker.CouldAttack) {
                _attacker.Attack();
                _avatar.PlayAutoPunch();
            }
        }
    }

    private void TranslateTo(PlayerState state) {
        ExitState(_currentState);
        _currentState = state;
        EnterState(_currentState);
    }

    private void EnterState(PlayerState state) {
        this.Log("Enter " + state, _shouldLog);
        switch (state) {
            case PlayerState.Stunned: {
                    _avatar.FallDown();
                    break;
                }
            case PlayerState.Death: {
                    DisableComponents();
                    break;
                }
            default: break;
        }
    }

    private void ExitState(PlayerState state) {
        this.Log("Exit " + state, _shouldLog);
        switch (state) {
            case PlayerState.Stunned: {
                    _avatar.StandUp();
                    break;
                }
            default: break;
        }
    }

    private void OnDeath() => TranslateTo(PlayerState.Death);

    private void OnStunStart() => TranslateTo(PlayerState.Stunned);

    private void OnStunEnd() => TranslateTo(PlayerState.Free);

    private void DisableComponents() {
        _attacker.Disable();
        _mover.Disable();
        _rotator.Disable();
        _damageabler.Disable();
        _deathabler.Disable();
        _stunnabler.Disable();
    }

    private void EnableComponents() {
        _attacker.Enable();
        _mover.Enable();
        _rotator.Enable();
        _damageabler.Enable();
        _deathabler.Enable();
        _stunnabler.Enable();
    }
}
