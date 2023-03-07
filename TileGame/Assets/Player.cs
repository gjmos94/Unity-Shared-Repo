using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Agent
{
    // set attack component to use in editor
    public Transform _attack;
    private IAttack _attackComponent;

    protected override void Start()
    {
        // call Agent.Start
        base.Start();

        // cache attack script
        _attackComponent = _attack.GetComponent<IAttack>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            // path to new location
            UpdatePathMove((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
        } else {
            // contiune movement along current path (or none)
            UpdatePathMove();
        }

        // if attack is useable, trigger on space
        if (!_attackComponent.isActive() && !_attackComponent.onCooldown() && Input.GetKeyDown(KeyCode.Space)) {
            // REM: assumes attack is to a position, need to refactor for, e.g. click-on-entity attacks
            _attackComponent.BehaviorUpdate(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            
            if (_attackComponent.stopsMovement()) {
                StopPathMove();
            }
        }
    }
}
