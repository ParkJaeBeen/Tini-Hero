using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTakeOffScript : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //RedDragonScript.instance.startCor(RedDragonScript.instance.IncreaseYPos());
        if (stateInfo.normalizedTime >= 0.15f)
        {
            RedDragonScript.instance.transform.position =
                Vector3.MoveTowards(RedDragonScript.instance.transform.position, RedDragonScript.instance.upperPosP, Time.deltaTime * 5.0f);
        }

        if(RedDragonScript.instance.transform.position == RedDragonScript.instance.upperPosP)
        {
            animator.SetInteger("aniInt", 7);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
