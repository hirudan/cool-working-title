﻿using BulletManagement;
using SlipTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actor
{
    /// <summary>
    /// Function that keeps track of health and 
    /// any resources dealing with morbid death.
    /// </summary>
    public class Living : MonoBehaviour, ISlipTimeAdherent
    {
        public SlipTimeManager SlipTimeManager => slipTimeManager;

        public Color damageTint;

        public int damageLayer = 10;

        // Private Variables
        [SerializeField]
        private SlipTimeManager slipTimeManager;
        
        [SerializeField]
        private int health = 100;

        [SerializeField]
        private Animator animator;
        
        [SerializeField]
        private float colorDecayTime = 2.0f;
        
        private SpriteRenderer spriteRenderer;
        private bool takenDamage = false;
        private float timeCounter = 0;
        private bool died = false;

        // Getters and Setters
        public int Health => this.health;
        public Color DamageTint => this.damageTint;
        public float ColorDecayTime => this.colorDecayTime;

        protected void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (health <= 0 && !died)
            {
                died = true;
                Die();
                return;
            }

            // Basic damage effect via scripts
            // we can decide to use sprites instead later down the line for crazier effects

            if (died || !takenDamage) return;
            Color lerpedColor = Color.Lerp(damageTint, Color.white, timeCounter);
            spriteRenderer.color = lerpedColor;
            timeCounter += (Time.deltaTime * this.SlipTimeManager.slipTimeCoefficient) / colorDecayTime;

            if (!(timeCounter >= colorDecayTime)) return;
            animator.ResetTrigger("Damage");
            takenDamage = false;
            timeCounter = 0f;
        }

        public bool IsDead()
        {
            return died;
        }

        public void TakeDamage(int amt)
        {
            // Cooldown period
            if (died || takenDamage)
            {
                return;
            }

            health -= amt;
            // Use basic red color tint for now instead of a new sprite
            spriteRenderer.color = damageTint;
            takenDamage = true;
            animator.SetTrigger("Damage");
        }

        protected virtual void Die()
        {
            // Clean up any bullets emitted by living entity via its name
            // which all emitters will grab as its parent.
            // This is very inefficient but is dirty and gets things done
            var bullets = GameObject.FindObjectsOfType<Bullet>();
            foreach (var b in bullets)
            {
                if (b.ownerName == gameObject.name)
                {
                    Destroy(b.gameObject);
                }
            }

            animator.SetTrigger("Die");
            // Disable any further damage effects
            animator.ResetTrigger("Damage");
            spriteRenderer.color = Color.white;
            Destroy(this.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject go = other.gameObject;
            if (go.layer == damageLayer)
            {
                // TODO: Different bullet damage should be registered here.
                TakeDamage(10);
                if (go.GetComponent<BulletManagement.Bullet>().destroyBulletOnHit)
                {
                    // Destroy the bullet that hit the object
                    Destroy(go);
                }
            }
        }
    }
}
