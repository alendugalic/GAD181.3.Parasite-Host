using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1000f)]
    public float maxHealth = 10f;

    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        if (HostMovement.blockInput)
        {
            damage *= 0.4f;
        }
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                Debug.Log("Object took " + damage + " damage. Current health: " + currentHealth + "Hit" + tag);
            }
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
