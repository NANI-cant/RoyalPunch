using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IFixedUpdateState {

    private IStateMachine _stateMachine;
    private Transform _lookAtIt;
    private IRotateable _rotator;
    private IAttackable _attacker;
    private EnemyAvatar _avatar;
    private SpellHandler _spellHandler;

    public EnemyIdleState(
        IStateMachine stateMachine,
        Transform lookAtIt,
        IRotateable rotateable,
        IAttackable attackable,
        EnemyAvatar avatar,
        SpellHandler spellHandler) {

        _stateMachine = stateMachine;
        _lookAtIt = lookAtIt;
        _rotator = rotateable;
        _attacker = attackable;
        _avatar = avatar;
        _spellHandler = spellHandler;
    }

    public void FixedUpdate() {
        _rotator.RotateTo(_lookAtIt.position);

        if (_attacker.CouldAttack) {
            _attacker.Attack();
            _avatar.PlayAttack();
        }

        if (_spellHandler.CouldCast()) {
            _spellHandler.CastRandom();
            _stateMachine.TranslateTo<EnemyCastSpellState>();
        }
    }
}
