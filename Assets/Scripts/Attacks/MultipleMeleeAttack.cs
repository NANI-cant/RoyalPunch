using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultipleMeleeAttack : Attack {
    [SerializeField] private List<MeleeAttack> _attacks;

    public override void Validate(MonoBehaviour attacker) {
        base.Validate(attacker);
        foreach (var attack in _attacks) {
            attack.Validate(attacker);
        }
    }

    public override List<Coroutine> Perform(MonoBehaviour attacker) {
        List<Coroutine> attackCoroutines = new List<Coroutine>();
        if (!CouldAttack(attacker)) return attackCoroutines;

        foreach (var attack in _attacks) {
            attackCoroutines.AddRange(attack.Perform(attacker));
        }
        return attackCoroutines;
    }

    public override bool CouldAttack(MonoBehaviour attacker) {
        foreach (var attack in _attacks) {
            if (!attack.CouldAttack(attacker)) {
                return false;
            }
        }
        return true;
    }

    public override void DrawGizmos(MonoBehaviour attacker) {
        base.DrawGizmos(attacker);
        foreach (var attack in _attacks) {
            attack.DrawGizmos(attacker);
        }
    }
}