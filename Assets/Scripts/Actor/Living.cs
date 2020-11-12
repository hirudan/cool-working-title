using SlipTime;
using UnityEngine;

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

        // Getters and Setters
        public int Health => this.health;
        public Color DamageTint => this.damageTint;
        public float ColorDecayTime => this.colorDecayTime;

        private void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (health <= 0)
            {
                Die();
            }

            // Basic damage effect via scripts
            // we can decide to use sprites instead later down the line for crazier effects
            if (takenDamage)
            {
                Color lerpedColor = Color.Lerp(damageTint, Color.white, timeCounter);
                spriteRenderer.color = lerpedColor;
                timeCounter += (Time.deltaTime * this.SlipTimeManager.slipTimeCoefficient) / colorDecayTime;

                if (timeCounter >= colorDecayTime)
                {
                    animator.ResetTrigger("Damage");
                    takenDamage = false;
                    timeCounter = 0f;
                }
            }
        }

        public bool IsDead()
        {
            return health <= 0;
        }

        public void TakeDamage(int amt)
        {
            // Cooldown period
            if (takenDamage)
            {
                return;
            }

            health -= amt;
            // Use basic red color tint for now instead of a new sprite
            spriteRenderer.color = damageTint;
            takenDamage = true;
            animator.SetTrigger("Damage");
        }

        private void Die()
        {
            animator.SetTrigger("Die");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 10)
            {
                TakeDamage(10);
            }
        }
    }
}
