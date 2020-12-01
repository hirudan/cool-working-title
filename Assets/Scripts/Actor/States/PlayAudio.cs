using UnityEngine;
using UnityEngine.Audio;

namespace Actor.States
{
    public class PlayAudio : StateMachineBehaviour
    {
        public AudioClip audio;
        public string audioGroup = "FX";
        public float volume = 1.0f;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var audioSource = animator.GetComponent<AudioSource>();
            AudioMixer mixer = Resources.Load("Master") as AudioMixer;
            audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups(audioGroup)[0];
            audioSource.clip = audio;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }
}
