using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Awareness : MonoBehaviour
{
    // keep track of single target
    private Transform m_target;
    public Transform Target
    {
        // C# public property access
        get { return m_target; }
        set { m_target = value; }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // target acquired
        m_target = collider.transform;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        // target lost
        if (m_target == collider.transform) {
            m_target = null;
        }
    }
}