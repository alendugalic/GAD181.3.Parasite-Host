
using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    public float speed = 5f;
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 3f; // Time between attacks
    private float timeSinceLastAttack = 0f;
    public Animator animator;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
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
                    Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;

                    if (distanceToPlayer > attackRange)
                    {
                        // Move towards the player if not in attack range
                        //animator.SetBool("isAttacking", false);
                        transform.Translate(direction * speed * Time.deltaTime);
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
                    }
                }
            }
        }
    }

    void AttackPlayer()
    {
        // Perform attack actions here
        Debug.Log("Attacking player!");

        // Play the attack animation
        animator.SetBool("isAttacking", true);
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

}
