using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_onShowFaceEnd : StateMachineBehaviour
{
    [SerializeField] childState nextState;
    script_childBehaviour cb;
    Rigidbody rb;
    script_maskController mc;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cb = anim.GetComponentInParent<script_childBehaviour>();

        rb = anim.GetComponentInParent<Rigidbody>();

        mc = anim.GetComponentInChildren<script_maskController>();
        mc.free();

        rb.isKinematic = false;

        bool cond = cb.asthmatic;
        if (cond) nextState = childState.idle;

        switch (nextState)
        {
            case childState.idle:
                anim.SetTrigger("idle");
                break;

            case childState.walk:
                anim.SetTrigger("walk");
                break;

            case childState.run:
                cb.startRunning(true);
                anim.SetTrigger("run");
                break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //cb = anim.GetComponent<script_childBehaviour>();
        
        bool cond = cb.asthmatic;
        if (cond)
        {
            cb.saved = true;
            nextState = childState.idle;
            script_gameController.winGame();
        }
        cb.state = nextState;
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
