using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    // all agents can set move speed in inspector
    public float MOVE_SPEED;

    // method to return move speed provides a central place
    //  to implement movement speed modifiers
    public float GetMoveSpeed() { return MOVE_SPEED; }

    ////////////////
    // path-based movement

    private UnityEngine.AI.NavMeshPath path;
    private int stepi = 0;

    protected virtual void Start()
    {
        // only need one path data structure at a time, so re-use
        path = new UnityEngine.AI.NavMeshPath();
    }

    // may be called with a new end point or the same end point
    // return true if moving along path, false if not (no path or path complete)
    public bool UpdatePathMove(Vector2 end)
    {

        // if we have no path or it is a new endpoint, calculate a new path to it
        if (path.corners.Length == 0 || (Vector2)path.corners[path.corners.Length-1] != end)
        {
            if (!UnityEngine.AI.NavMesh.CalculatePath((Vector2)transform.position, end, UnityEngine.AI.NavMesh.AllAreas, path)) {
                // no path found
                return false;
            }
            // path corner[0] is the starting point, first waypoint is corner[1]
            stepi = 1;
        }

        return UpdatePathMove();
    }

    // when called with no end point, continue ongoing movement (or do nothing)
    // return true if moving along path, false if not (no path or path complete)
    public bool UpdatePathMove()
    {
        // no path or path is finished
        if (stepi >= path.corners.Length)
        {
            return false;
        }

        // move towards next waypoint, advance to next next waypoint on arrival
        Vector2 leg = path.corners[stepi] - transform.position;
        if (leg.magnitude < GetMoveSpeed() * Time.deltaTime) {
            transform.position = path.corners[stepi];
            stepi++;
        } else {
            transform.position += (Vector3)leg.normalized * GetMoveSpeed() * Time.deltaTime;
        }

        // draw path in scene for debugging
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);

        // always succeeds if we have a path
        return true;
    }

    // called to clear current path and stop ongoing movement
    public void StopPathMove() { path.ClearCorners(); }
}
