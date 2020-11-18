using Player;
using UnityEngine;

namespace Collectables
{
    /// <summary>
    /// A drop that increases the player's score.
    /// </summary>
    public class ScoreDrop : MonoBehaviour
    {
        /// <summary>
        /// The amount to increase the player's score by.
        /// </summary>
        public int scoreBonus = 100;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 8)
            {
                PlayerStats.Score += scoreBonus;
            }
        }
    }
}
