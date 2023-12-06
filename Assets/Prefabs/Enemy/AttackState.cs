using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    //List<Transform> players = new List<Transform>();

    //// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Assume there is a GameManager that keeps track of players
    //    GameManager gameManager = FindObjectOfType<GameManager>();

    //    if (gameManager != null)
    //    {
    //        players = gameManager.GetPlayersTransforms();
    //        Debug.Log("Found targets");
    //    }
    //    else
    //    {
    //        Debug.LogError("GameManager not found");
    //    }
    //}

    //// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (players.Count > 0)
    //    {
    //        Transform closestPlayer = GetClosestPlayer(animator.transform.position);
    //        if (closestPlayer != null)
    //        {
    //            animator.transform.LookAt(closestPlayer);
    //            float distance = Vector3.Distance(closestPlayer.position, animator.transform.position);
    //            if (distance > 3.5)
    //                animator.SetBool("isAttacking", false);
    //        }
    //    }
    //}

    //// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    //// Helper method to find the closest player
    //private Transform GetClosestPlayer(Vector3 enemyPosition)
    //{
    //    Transform closestPlayer = null;
    //    float closestDistance = float.MaxValue;

    //    foreach (Transform playerTransform in players)
    //    {
    //        float distance = Vector3.Distance(playerTransform.position, enemyPosition);
    //        if (distance < closestDistance)
    //        {
    //            closestDistance = distance;
    //            closestPlayer = playerTransform;
    //        }
    //    }

    //    return closestPlayer;
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
