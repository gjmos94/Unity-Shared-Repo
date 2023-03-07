using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSlash : MonoBehaviour, IAttack
{
    // animation timing, need the clip(s) to get their length dynamically
    public AnimationClip _slash;

    // set duration of each of the child slash animations and the delay between starting each one
    public float SLASH_DELAY = 0.05f;
    public float SLASH_DURATION = 0.3f;

    // setting dmg at the attack level in editor for simplicity
    // could be a dynamic calculation based on attack, weapon, and player stats (routed through agent)
    public int DAMAGE = 1;

    // internal state
    private Agent _agent;
    private Transform _parent;
    private bool _isActive = false;
    private List<Collider2D> _hitList = new List<Collider2D>();

    void Start()
    {
        _agent = GetComponentInParent<Agent>();

        // cache parent to re-attach (not assuming that the immediate parent is the agent entity)
        _parent = transform.parent;
        
    }

    // IAttack info for this attack
    public bool isActive() { return _isActive; }
    public bool onCooldown() { return false; }
    public bool stopsMovement() { return true; }

    // when called with no target, continue the attack, return false if not attacking or done
    public bool BehaviorUpdate()
    { 
        // because this attack uses a coroutine, frame update happens automatically
        // just return true/false based on whether the attack is still happening
        return _isActive;
    }

    // when called with transform or position, attack in that direction
    // return true if attacking, false if not attacking or done
    public bool BehaviorUpdate(Transform targetEntity) { return BehaviorUpdate(targetEntity.position); }
    public bool BehaviorUpdate(Vector2 targetPosition)
    {
        if (_isActive) {
            // already running, nothing to do but report status
            return true;
        }

        // calculate the direction to that target position from agent position
        Vector2 targetDir = targetPosition - (Vector2)_agent.transform.position;

        // use to orient the attack
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.SignedAngle(Vector2.right, targetDir));
        // unparent (this attack fires in place, does not follow moving entity)
        transform.parent = null;

        // start new attack using coroutine (async sugar over frame update)
        StartCoroutine(SlashAll());
        
        return true;
    }

    IEnumerator SlashAll()
    {

        // clear list of entities hit
        _hitList.Clear();

        // start each slash with DELAY between
        _isActive = true;
        for (int i=0;i<transform.childCount;i++) {
            Transform child = transform.GetChild(i);
            StartCoroutine(Slash(child));
            // wait for last one to finish
            yield return new WaitForSeconds(i == (transform.childCount-1) ? SLASH_DURATION : SLASH_DELAY);
        }

        // all complete, reparent and mark done
        transform.parent = _parent;
        transform.localPosition = Vector2.zero;
        _isActive = false;
    }

    // nested coroutine to start and clean up each little circle hit
    IEnumerator Slash(Transform child)
    {
        // activate and start animation
        child.gameObject.SetActive(true);
        // scales clip to take SLASH_DURATION time
        Animator anim = child.GetComponent<Animator>();
        anim.speed = _slash.length / SLASH_DURATION;
        anim.Play(_slash.name, 0);

        // wait until finshed and clean up
        yield return new WaitForSeconds(SLASH_DURATION);
        child.gameObject.SetActive(false);
    }

    // collider triggers on overlap with colliders on the Entity layer
    void OnTriggerEnter2D(Collider2D collider)
    {
        // hti list prevents hitting same entity multiple times in one attack
        if (!_hitList.Contains(collider)) {

            // get other agent for tag and hp
            Agent other = collider.GetComponentInParent<Agent>();
            // simple rule: can't hit entities with the same tag as you (including yourself!)
            if (other.tag != _agent.tag) {
                HealthBar hp = other.GetComponentInChildren<HealthBar>();
                if (hp) {
                    hp.ChangeHealth(-1 * DAMAGE);
                }
            }
            _hitList.Add(collider);
        }
    }
}
