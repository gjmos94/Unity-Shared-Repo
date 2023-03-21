using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Agent
{
    public Transform _attack;
    private IAttack _attackComponent;

    // behaviors
    private Patrol _patrol;
    private Awareness _aware;
    private Chase _chase;
    private Flee _flee;
    // choose-to-attack cooldown
    float COOLDOWN = 4.0f;      // <=======updated from 2.0f to 4.0f
    float _attackCooldownTimer = 0f;

    protected override void Start()
    {
        // call Agent.Start
        base.Start();

        // cache attack script
        _attackComponent = _attack.GetComponent<IAttack>();

        // cache behavior scripts
        _aware = GetComponentInChildren<Awareness>();
        _patrol = GetComponent<Patrol>();
        _chase = GetComponent<Chase>();
        _flee = GetComponent<Flee>();
    }

    void Update()
    {
        // implement cooldown on choosing to attack (not super fun, but better than spamming)
        if (_attackCooldownTimer > 0)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }

        /////////////////////////////////////////////////////
        // reactive behavior stack

        // priority 1: if currently attacking, keep doing so
        if (_attackComponent.isActive() && _attackComponent.BehaviorUpdate())
        {
            // empty body for the time being because there's no additional work to do when the attack is ongoing
        }

        // priority 2: if there's a target and our cooldown is up, try start a new attack
        else if (_aware.Target && _attackCooldownTimer <= 0 && _attackComponent.BehaviorUpdate(_aware.Target))
        {
            // when successful starting an attack, start the cooldown as well
            _attackCooldownTimer = COOLDOWN;
        }

        // priority 3: try chase the current target (if target is null, chase fails)
        else if (_flee.BehaviorUpdate(_aware.Target)) { }

        // priority 4: nothing else to do, go back to patroling
        else _patrol.BehaviorUpdate();
    }
}
