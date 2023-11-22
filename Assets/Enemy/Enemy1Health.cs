using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Health : MonoBehaviour
{
    public int HP= 100;
    public Animator animator;

public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if(HP <= 0)
        {
            AudioManager.instance.Play("Dragon_Death");
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;   
        }
        else
        {
            AudioManager.instance.Play("Dragon_Damage");
            animator.SetTrigger("damage");
        }
    }
}
