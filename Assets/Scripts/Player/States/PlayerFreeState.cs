using System;
using UnityEngine;

public class PlayerFreeState : IUpdateState, IFixedUpdateState, IEnterState, IExitState {
    private IStateMachine _stateMachine;
    private Transform _lookAtThis;
    private IMovable _mover;
    private IRotateable _rotator;
    private IAttackable _attacker;
    private PlayerAvatar _avatar;
    private IInputService _input;
    private IStunnable _stunnable;

    public PlayerFreeState(
        IStateMachine stateMachine,
        IInputService input,
        PlayerAvatar avatar,
        IAttackable attacker,
        IRotateable rotator,
        IMovable mover,
        Transform lookAtThis,
        IStunnable stunnable) {
        _stateMachine = stateMachine;
        _input = input;
        _avatar = avatar;
        _attacker = attacker;
        _rotator = rotator;
        _mover = mover;
        _lookAtThis = lookAtThis;
        _stunnable = stunnable;
    }

    public void Enter() {
        _stunnable.OnStunStart += OnStunStart;
    }

    public void Exit() {
        _stunnable.OnStunStart -= OnStunStart;
    }

    public void Update() {
        _avatar.PlayMoveInDirection(_input.Direction);
    }

    public void FixedUpdate() {
        _mover.Move(_input.Direction.To3Dimentions());
        _rotator.RotateTo(_lookAtThis.position);

        if (_attacker.CouldAttack) {
            _attacker.Attack();
            _avatar.PlayAutoPunch();
        }
    }

    private void OnStunStart() {
        _stateMachine.TranslateTo<PlayerStunState>();
    }
}