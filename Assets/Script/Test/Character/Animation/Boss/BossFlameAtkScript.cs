using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlameAtkScript : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RedDragonScript.instance.startCor(RedDragonScript.instance.flame());
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= 0.97f)
        {
            animator.SetInteger("aniInt", 0);
            RedDragonScript.instance.flameThrowP.SetActive(false);
            //RedDragonScript.instance.FlameTriggerP = false;
            RedDragonScript.instance.FlameThrowCoolTimeP = 0.0f;
            RedDragonScript.instance.stopCor(RedDragonScript.instance.flame());
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RedDragonScript.instance.FlameTriggerP = false;
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
