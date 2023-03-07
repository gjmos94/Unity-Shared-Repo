using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol2 : MonoBehaviour
{
    // patrol between points specified in editor
    public Transform[] _waypoints;
    private int _nexti = 0;
    private Agent _agent;

    void Start()
    {
        // cache agent on same or parent entity
        _agent = GetComponentInParent<Agent>();
    }

    public bool BehaviorUpdate()
    {
        if (!_agent.UpdatePathMove(_waypoints[_nexti].position))
        {
            // arrived next waypoint (cycle)
            _nexti = (_nexti + 1) % _waypoints.Length;
        }
        return true;
    }
}
