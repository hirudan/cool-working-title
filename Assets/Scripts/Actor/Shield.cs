using Player;
using SlipTime;
using UnityEngine;

namespace Actor
{
    /// <summary>
    /// The player's shield. Used to manage bullet grazing behavior.
    /// </summary>
    public class Shield : MonoBehaviour
    {
        public SlipTimeManager slipTimeManager;

        public AudioSource bulletGrazeAudio;

        public int bulletGrazeBonusScore = 10;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 10)
            {
                bulletGrazeAudio.Play();
                
                // Award bullet grazing bonus.
                PlayerStats.Score += bulletGrazeBonusScore;
                
                // Award SlipTime cooldown decrease bonus.
                slipTimeManager.DecreaseCooldown();
            }
        }
    }
}
