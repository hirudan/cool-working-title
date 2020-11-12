using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActorState
{
    public class PlayAudio : StateMachineBehaviour
    {
        public AudioClip audio;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AudioSource audioSource = animator.GetComponent<AudioSource>();
            audioSource.clip = audio;
            audioSource.Play();
        }
    }
}
