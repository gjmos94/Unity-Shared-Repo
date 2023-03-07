using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    // attack by position or entity transform
    public bool BehaviorUpdate(Vector2 targetPosition);
    public bool BehaviorUpdate(Transform targetEntity);
    // with no target, continue current attack (or none)
    public bool BehaviorUpdate();

    // attack must return information useful for decision-making
    public bool isActive();
    public bool onCooldown();
    public bool stopsMovement();
}
