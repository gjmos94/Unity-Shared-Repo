using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{
    private Transform _target;
    private Agent _agent;
    
    void Start()
    {
        // cache agent on same or parent entity
        _agent = GetComponentInParent<Agent>();
    }

    // may be called w/ target or null
    // if null, continue chasing the target we have (if we have one)
    public bool BehaviorUpdate(Transform target)
    {
        if (target != null && target.gameObject.tag == "Player") {  // 
            // update to new target
            _target = target;
        }

        // have no target, nothing to do
        if (_target == null) {
            return false;
        }

        // if we have a target, try move towards it
        if (!_agent.UpdatePathMove(_target.position)) {
            // unable to move towards it (path failed or already there), clear target
            _target = null;
            return false;
        }
        // chase succeeded and ongoing
        return true;
    }
}
