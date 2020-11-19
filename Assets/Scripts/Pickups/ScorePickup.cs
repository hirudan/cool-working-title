using Player;
using UnityEngine;

namespace Pickups
{
    /// <summary>
    /// A drop that increases the player's score.
    /// </summary>
    public class ScorePickup : Pickup
    {
        /// <summary>
        /// The amount to increase the player's score by.
        /// </summary>
        public int scoreBonus = 100;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 8)
            {
                PlayerStats.Score += scoreBonus;
                Destroy(gameObject);
            }
        }
    }
}
