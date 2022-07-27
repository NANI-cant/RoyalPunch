using System;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[RequireComponent(typeof(IDamageable))]
[RequireComponent(typeof(IDeathable))]
[RequireComponent(typeof(IRotateable))]
[RequireComponent(typeof(IAttackable))]
[RequireComponent(typeof(EnemyAvatar))]
[RequireComponent(typeof(SpellHandler))]
[RequireComponent(typeof(IStunnable))]
public class Enemy : MonoBehaviour, IStateMachine {
    [SerializeField] private bool _shouldLog = false;

    private Transform _lookAtIt;

    private Game _game;
    private IDamageable _damageabler;
    private IDeathable _deathabler;
    private IRotateable _rotator;
    private IAttackable _attacker;
    private IStunnable _stunnabler;
    private EnemyAvatar _avatar;
    private SpellHandler _spellHandler;

    private IState _activeState;
    private Dictionary<Type, IState> _states;

    [Inject]
    private void Constructor(Player player, Game game) {
        _game = game;
        _lookAtIt = player.transform;
        _damageabler = GetComponent<IDamageable>();
        _deathabler = GetComponent<IDeathable>();
        _rotator = GetComponent<IRotateable>();
        _attacker = GetComponent<IAttackable>();
        _stunnabler = GetComponent<IStunnable>();
        _avatar = GetComponent<EnemyAvatar>();
        _spellHandler = GetComponent<SpellHandler>();

        _states = new Dictionary<Type, IState> {
            [typeof(EnemyPrepareState)] = new EnemyPrepareState(this, game),
            [typeof(EnemyIdleState)] = new EnemyIdleState(this, _lookAtIt, _rotator, _attacker, _avatar, _spellHandler),
            [typeof(EnemyCastSpellState)] = new EnemyCastSpellState(this, _spellHandler, _stunnabler),
            [typeof(EnemyStunState)] = new EnemyStunState(this, _avatar, _stunnabler),
            [typeof(EnemyWinState)] = new EnemyWinState(this),
            [typeof(EnemyLoseState)] = new EnemyLoseState(this, _avatar),
        };
        TranslateTo<EnemyPrepareState>();
    }

    private void OnEnable() {
        _game.OnGameLoose += OnGameLoose;
        _game.OnGameWin += OnGameWin;
    }

    private void OnDisable() {
        _game.OnGameLoose -= OnGameLoose;
        _game.OnGameWin -= OnGameWin;
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

    private void OnGameWin() => TranslateTo<EnemyLoseState>();
    private void OnGameLoose() => TranslateTo<EnemyWinState>();
}
