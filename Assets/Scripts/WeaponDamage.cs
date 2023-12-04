using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField]
    [Header("The damage of each individual Hit")]
    [Range(0.1f, 1000f)]
    public float damage = 10f;
    private void OnCollisionEnter(Collision other)
    {

        // Check if the collided object has the "Enemy" tag.
        if (other.gameObject.CompareTag("Player"))
        {
            // Deal damage to the enemy (e.g., reduce enemy's health by 10).
            Health playerHealth = other.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);

            }

        }
    }

}
