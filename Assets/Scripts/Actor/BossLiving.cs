using Player;
using UnityEngine;

namespace Actor
{
    public class BossLiving : Living
    {
        public new int damageLayer = 11;

        public int hitScoreBonus = 50;
        
        public int deathScoreBonus = 100;

        protected override void Die()
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
                if (go.GetComponent<BulletManagement.Bullet>().destroyBulletOnHit)
                {
                    // Destroy the bullet that hit the object
                    Destroy(go);
                }
            }
        }
    }
}
