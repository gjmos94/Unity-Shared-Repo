using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : MonoBehaviour
{
    private Transform _target;
    private Agent _agent;
    public int heal = 10;
    private List<Collider2D> _hitList = new List<Collider2D>();
    void Start()
    {
        // cache agent on same or parent entity
        _agent = GetComponentInParent<Agent>();
    }

    // may be called w/ target or null
    // if null, continue chasing the target we have (if we have one)
    public bool BehaviorUpdate(Transform target)
    {
        if (target != null && target.gameObject.tag == "Player")
        {
            // update to new target
            _target = target;
        }

        // have no target, nothing to do
        if (_target == null)
        {
            return false; 
        }

        // if we have a target, try move away from it
        Vector2 fleeDir = -1 * (_target.position - _agent.transform.position);
        if(!_agent.UpdatePathMove((Vector2)_agent.transform.position + fleeDir))
        {
            Debug.Log("IMRUNNING");
            // unable to flee!
            return false;
        }
        // flee succeeded and ongoing
        return true;

      
    }
    void OnTriggerEnter2D(Collider2D collider)
    {

        if (!_hitList.Contains(collider))
        {

            // get other agent for tag and hp
            Agent other = collider.GetComponentInParent<Agent>();
            // simple rule: can't hit entities with the same tag as you (including yourself!)
            if (other.tag != _agent.tag)
            {
                HealthBar hp = other.GetComponentInChildren<HealthBar>();
                if (hp)
                {
                    hp.ChangeHealth(10 * heal);
                }
            }
            _hitList.Add(collider);
            _hitList.Clear();
        }
        _hitList.Clear();
    }
        void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            BehaviorUpdate(player.transform);
        }
    }
}
