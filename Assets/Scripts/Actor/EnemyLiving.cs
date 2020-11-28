using BulletManagement;
using Player;
using UnityEngine;

namespace Actor
{
    public class EnemyLiving : Living
    {
        public int hitScoreBonus = 1;

        public int deathScoreBonus = 1000;

        private new void Start()
        {
            // Set the layer from which to take damage to the player's bullets.
            // We do this in Start() instead of hiding the parent field with the "new" keyword
            // because Unity complains about serialization of multiple of the same field if we do so.
            damageLayer = 11;
            base.Start();
        }

        public override void Die()
        {
            PlayerStats.Score += deathScoreBonus;
            base.Die();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject go = other.gameObject;
            if (go.layer == damageLayer)
            {
                TakeDamage(10);
                PlayerStats.Score += hitScoreBonus;
                if (go.GetComponent<Bullet>().destroyBulletOnHit)
                {
                    // Destroy the bullet that hit the object
                    Destroy(go);
                }
            }
        }
    }
}
