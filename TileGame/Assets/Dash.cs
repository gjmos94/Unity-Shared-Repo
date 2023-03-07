using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashDistance = 5f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 6f;

    private Agent agent;
    private bool isDashing = false;
    private Vector2 dashTarget = Vector2.zero;
    private float dashCooldownTimer = 0f;

    void Start()
    {
        agent = GetComponent<Agent>();
        dashCooldownTimer = dashCooldown;
    }

    void Update()
    {
        if (isDashing)
        {
            // Move the agent towards the dash target
            agent.UpdatePathMove(dashTarget);

            // Check if the agent has reached the dash target
            float distanceToTarget = Vector2.Distance(transform.position, dashTarget);
            if (distanceToTarget < 1.0f)
            {
                // Stop dashing and reset cooldown timer
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }
        }
        else
        {
            // Count down the dash cooldown timer
            if (dashCooldownTimer > 0)
            {
                dashCooldownTimer -= Time.deltaTime;
            }
            else
            {
                // Start dashing towards the player
                dashTarget = FindObjectOfType<Player>().transform.position;
                Vector2 dashDirection = (dashTarget - (Vector2)transform.position).normalized;
                dashTarget = (Vector2)transform.position + dashDirection * dashDistance;
                agent.UpdatePathMove(dashTarget);
                isDashing = true;
            }
        }
    }
}