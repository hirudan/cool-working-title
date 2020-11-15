using UnityEngine;

namespace Actor.States
{
    public class Died : StateMachineBehaviour
    {
        public AudioClip audio;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var audioSource = animator.GetComponent<AudioSource>();
            audioSource.clip = audio;
            audioSource.Play();
        }
    }
}
