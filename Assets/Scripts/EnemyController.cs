
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    public float speed = 5f;
    public float detectionRange = 6f;
    public float attackRange = 1.5f;
    public float attackCooldown = 3f; // Time between attacks
    private float timeSinceLastAttack = 0f;
    public Animator animator;

    // Limit the enemy movement within a circle of this radius
    public float patrolRadius = 6f;
    private Vector3 patrolPoint;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Initialize the patrol point
        UpdatePatrolPoint();
    }

    void Update()
    {
        if (!IsServer)
            return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 0)
        {
            GameObject targetPlayer = GetClosestPlayer(players);

            if (targetPlayer != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

                if (distanceToPlayer <= detectionRange)
                {
                    Vector3 direction;

                    if (distanceToPlayer > attackRange)
                    {
                        // Move towards the patrol point if not in attack range
                        direction = (patrolPoint - transform.position).normalized;

                        // Set walking animation
                        animator.SetBool("isWalking", true);
                    }
                    else
                    {
                        // Check if enough time has passed since the last attack
                        if (Time.time - timeSinceLastAttack > attackCooldown)
                        {
                            // Attack when in range and cooldown is over
                            AttackPlayer();
                            timeSinceLastAttack = Time.time; // Record the time of the attack
                        }

                        // Stay in place when attacking
                        direction = Vector3.zero;

                        // Set walking animation to false when attacking
                        animator.SetBool("isWalking", false);
                    }

                    // Move the enemy
                    transform.Translate(direction * speed * Time.deltaTime);

                    // If the enemy has reached the patrol point, update it
                    if (Vector3.Distance(transform.position, patrolPoint) < 0.2f)
                    {
                        UpdatePatrolPoint();
                    }
                }
            }
        }
        else
        {
            // Set walking animation to false when no players are detected
            animator.SetBool("isWalking", false);
        }
    }

    void AttackPlayer()
    {
        // Perform attack actions here
        Debug.Log("Attacking player!");

        // Play the attack animation
        animator.SetBool("isAttacking", true);

        // Set walking animation to false when attacking
        animator.SetBool("isWalking", false);
    }

    GameObject GetClosestPlayer(GameObject[] players)
    {
        GameObject closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    void UpdatePatrolPoint()
    {
        // Generate a random point within the patrol radius
        patrolPoint = transform.position + UnityEngine.Random.onUnitSphere * patrolRadius;
        patrolPoint.y = transform.position.y; // Ensure the patrol point is at the same height as the enemy
    }
}
