using System;
using UnityEngine;
using UnityEngine.UI;

namespace Actor
{
    public class Boss: MonoBehaviour
    {
        // A list of attack patterns that can contain non-spells and spells
        public GameObject[] attacks;

        // How long to wait before switching to a new attack pattern
        public float attackSwitchCooldownSeconds = 2f;

        // The attack pattern currently active
        public BossAttack currentAttack;

        private int currentAttackIndex = 0;

        private float timeElapsed = 0f;
        
        /// <summary>
        /// The score text box to update and display.
        /// </summary>
        public Text attackNameText;

        // Damage taken during the current attack
        private int damageTaken = 0;

        private bool inAttack = false;

        private Living vitality;

        // The health at the beginning of the current attack
        private int baselineHealth;

        void Start()
        {
            currentAttack = attacks[0].GetComponent<BossAttack>();
            vitality = gameObject.GetComponent<Living>();
            baselineHealth = vitality.Health;
            inAttack = true;
            currentAttack = Instantiate(currentAttack);
            UpdateNameTextbox();
        }

        private void Update()
        {
            if (vitality.Health <= 0) return;
            // First, handle cooldowns between attacks
            if (!inAttack)
            {
                if (timeElapsed > attackSwitchCooldownSeconds)
                {
                    // If we've passed the cooldown period, initiate the next attack
                    timeElapsed = 0;
                    inAttack = true;
                    currentAttack = Instantiate(currentAttack);
                    UpdateNameTextbox();
                    return;
                }
                else
                {
                    // Otherwise, wait a bit longer
                    timeElapsed += Time.deltaTime;
                    return;
                }
            }

            try
            {
                // Check if the player has timed out or has killed this phase of the boss
                if (timeElapsed > currentAttack.durationSeconds || damageTaken >= currentAttack.health)
                {
                    // Dispose of current attack's emitters
                    currentAttack.CleanUp();
                    // Reset damage counters
                    baselineHealth = vitality.Health; // re-baseline health for next attack
                    damageTaken = 0;
                    // Prepare to move to next attack
                    currentAttackIndex++;
                    currentAttack = attacks[currentAttackIndex].GetComponent<BossAttack>();
                    // Enter cooldown cycle
                    timeElapsed = 0f;
                    inAttack = false;
                }
                else
                {
                    // Update the player's progress
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

        /// <summary>
        /// Populates the attack name text box when applicable
        /// </summary>
        private void UpdateNameTextbox()
        {
            var updateText = string.IsNullOrEmpty(currentAttack.attackName) ? "" : currentAttack.attackName;
            attackNameText.text = updateText;
        }
    }
}
