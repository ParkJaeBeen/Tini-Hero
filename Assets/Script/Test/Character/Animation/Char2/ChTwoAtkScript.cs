using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChTwoAtkScript : StateMachineBehaviour
{
    CharTwoScript charTwoScript;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetFloat("AttackSpeed", animator.GetFloat("AttackSpeed") + PlayerManager.instance.charTwoScriptPublic.);
        charTwoScript = PlayerManager.instance.charTwoScriptPublic;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.2f)
            charTwoScript.Arrow.SetActive(true);

        if (stateInfo.normalizedTime >= 0.7f)
        {
            charTwoScript.Arrow.SetActive(false);
        }
            
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerManager.instance.playerTransform == PlayerManager.instance.playerTransformsPublic["Character_2"])
        {
            if (PlayerManager.instance.inputX == 0 && PlayerManager.instance.inputZ == 0)
            {
                animator.SetInteger("aniInt", 0);
            }
            else if (!(PlayerManager.instance.inputX == 0 && PlayerManager.instance.inputZ == 0))
            {
                animator.SetInteger("aniInt", 1);
            }
        }
        else
        {
            animator.SetInteger("aniInt", 0);
        }
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
