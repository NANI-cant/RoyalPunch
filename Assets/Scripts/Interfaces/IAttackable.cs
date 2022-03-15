using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable {
    bool CouldAttack { get; }
    void Attack();
}
