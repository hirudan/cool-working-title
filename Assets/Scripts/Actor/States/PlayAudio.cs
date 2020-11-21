using UnityEngine;

namespace Actor.States
{
    public class PlayAudio : StateMachineBehaviour
    {
        public AudioClip audio;
        public float volume = 1.0f;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var audioSource = animator.GetComponent<AudioSource>();
            audioSource.clip = audio;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }
}
