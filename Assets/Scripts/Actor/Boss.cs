using System;
using UnityEngine;

namespace Actor
{
    public class Boss: MonoBehaviour
    {
        // A list of attack patterns that can contain non-spells and spells
        public BossAttack[] attacks;

        // How long to wait before switching to a new attack pattern
        public float attackSwitchCooldownSeconds = 2f;

        // The attack pattern currently active
        private BossAttack currentAttack;

        private int currentAttackIndex = 0;

        private float timeElapsed = 0f;

        // Damage taken during the current attack
        private int damageTaken = 0;

        private bool inAttack = false;

        private Living vitality;

        // The health at the beginning of the current attack
        private int baselineHealth;

        void Start()
        {
            currentAttack = attacks[0];
            vitality = gameObject.GetComponent<Living>();
            baselineHealth = vitality.Health;
        }

        private void Update()
        {
            // First, handle cooldowns between attacks
            if (!inAttack && timeElapsed > attackSwitchCooldownSeconds)
            {
                timeElapsed = 0;
                inAttack = true;
                return;
            }

            try
            {
                if (timeElapsed > currentAttack.durationSeconds || damageTaken >= currentAttack.health)
                {
                    currentAttack.CleanUp();
                    currentAttackIndex++;
                    currentAttack = attacks[currentAttackIndex];
                    timeElapsed = 0f;
                    inAttack = false;
                    baselineHealth = vitality.Health; // re-baseline health for next attack
                }
                else
                {
                    timeElapsed += Time.deltaTime;
                    damageTaken = baselineHealth - vitality.Health;
                }
            }
            catch (IndexOutOfRangeException)
            {
                // If we catch an out of range exception, that means we finished the last attack
                vitality.TakeDamage(vitality.Health);
            }
        }
    }
}
