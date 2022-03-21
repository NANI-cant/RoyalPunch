using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable : IEnable, IDisable {
    bool CouldAttack { get; }
    void Attack();
}
