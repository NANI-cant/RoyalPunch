using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(IMovable))]
[RequireComponent(typeof(IRotateable))]
[RequireComponent(typeof(IAttackable))]
[RequireComponent(typeof(IDamageable))]
[RequireComponent(typeof(IDeathable))]
[RequireComponent(typeof(IStunnable))]
[RequireComponent(typeof(PlayerAvatar))]
public class Player : MonoBehaviour, IStateMachine {
    [SerializeField] private bool _shouldLog;

    private Transform _lookAtThis;
    private IInputService _input;
    private IMovable _mover;
    private IRotateable _rotator;
    private IAttackable _attacker;
    private IDamageable _damageabler;
    private IDeathable _deathabler;
    private IStunnable _stunnabler;
    private PlayerAvatar _avatar;

    private IState _activeState;
    private Dictionary<Type, IState> _states = new Dictionary<Type, IState>();

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

        _states = new Dictionary<Type, IState> {
            [typeof(PlayerFreeState)] = new PlayerFreeState(this, _input, _avatar, _attacker, _rotator, _mover, _lookAtThis, _stunnabler),
            [typeof(PlayerStunState)] = new PlayerStunState(this, _avatar, _stunnabler),
            [typeof(PlayerLoseState)] = new PlayerLoseState(this),
            [typeof(PlayerWinState)] = new PlayerWinState(this),
        };
        TranslateTo<PlayerFreeState>();
    }

    private void OnEnable() {
        _deathabler.OnDeath += OnDeath;
    }

    private void OnDisable() {
        _deathabler.OnDeath -= OnDeath;
    }

    private void Update() {
        _activeState?.Do(
            () => ((IUpdateState)_activeState).Update(),
            when: _activeState is IUpdateState
        );
    }

    private void FixedUpdate() {
        _activeState?.Do(
            () => ((IFixedUpdateState)_activeState).FixedUpdate(),
            when: _activeState is IFixedUpdateState
        );
    }

    public void TranslateTo<TState>() where TState : IState {
        _activeState?.Do(
            () => {
                ((IExitState)_activeState).Exit();
                this.Log("Exit " + _activeState, _shouldLog);
            },
            when: _activeState is IExitState
        );
        _activeState = _states[typeof(TState)];
        _activeState.Do(
            () => {
                ((IEnterState)_activeState).Enter();
                this.Log("Enter " + _activeState, _shouldLog);
            },
            when: _activeState is IEnterState
        );
    }

    private void OnDeath() => TranslateTo<PlayerLoseState>();
}
