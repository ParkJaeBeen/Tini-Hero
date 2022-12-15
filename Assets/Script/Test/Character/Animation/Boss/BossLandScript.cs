using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLandScript : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RedDragonScript.instance.lowerPosP = new Vector3(RedDragonScript.instance.transform.position.x,
            RedDragonScript.instance.transform.position.y - 15.0f,
            RedDragonScript.instance.transform.position.z);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.2f)
        {
            RedDragonScript.instance.transform.position =
                Vector3.MoveTowards(RedDragonScript.instance.transform.position, RedDragonScript.instance.lowerPosP, Time.deltaTime * 6.5f);
        }

        if (stateInfo.normalizedTime >= 0.8f)
        {
            RedDragonScript.instance.MeteorTriggerP = false;
            RedDragonScript.instance.BossCollider.enabled = true;
            animator.SetInteger("aniInt", 0);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RedDragonScript.instance.IsInvincibilityP = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
