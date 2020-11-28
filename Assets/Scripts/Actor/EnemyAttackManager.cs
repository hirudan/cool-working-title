using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Actor
{
    public class EnemyAttackManager: MonoBehaviour
    {
        /// <summary>
        /// A list of attack patterns that can contain non-spells and spells.
        /// </summary>
        public GameObject[] attacks;

        /// <summary>
        /// The attack pattern currently active.
        /// </summary>
        public EnemyAttack currentAttack;

        /// <summary>
        /// How long to wait before switching to a new attack pattern.
        /// </summary>
        public float attackSwitchCooldownSeconds = 2f;

        /// <summary>
        /// The score text box to update and display.
        /// </summary>
        [CanBeNull] public Text attackNameText;

        /// <summary>
        /// The amount of time left on the attack
        /// </summary>
        [CanBeNull] public Text timeRemaining;

        /// <summary>
        /// The health bar to display for the attack
        /// </summary>
        public Image healthBar;

        /// <summary>
        /// Whether the enemy should loop through its attacks.
        /// </summary>
        public bool shouldLoopAttacks;

        /// <summary>
        /// Whether the enemy should die if it runs out of attacks.
        /// </summary>
        public bool canBeTimedOut;

        // Whether the enemy has attacks to execute.
        private bool hasAttacksToExecute;

        // Fields to keep track of various state for this enemy.
        private float timeElapsed;
        private int damageTaken;
        private bool inAttack;
        private EnemyLiving vitality;
        private int attackIndex;

        // The health at the beginning of the current attack.
        public int baselineHealth;

        private void Start()
        {
            healthBar.type = Image.Type.Filled;
            vitality = gameObject.GetComponent<EnemyLiving>();
            baselineHealth = vitality.Health;
            inAttack = true;

            if (attacks.Length > 0)
            {
                hasAttacksToExecute = true;
                attackIndex = 0;
                currentAttack = attacks[attackIndex].GetComponent<EnemyAttack>();
                currentAttack = Instantiate(currentAttack);
                // Enable healthbar items for first attack
                healthBar.gameObject.SetActive(true);
                timeRemaining.gameObject.SetActive(true);
                attackNameText.gameObject.SetActive(true);
                UpdateNameTextbox();
            }
        }

        private void Update()
        {
            // Perform attack-related updates.
            if (!hasAttacksToExecute) return;
            // Handle cooldowns between attacks.
            if (!inAttack)
            {
                // If we've passed the cooldown period, initiate the next attack.
                if (timeElapsed > attackSwitchCooldownSeconds)
                {
                    timeElapsed = 0;
                    inAttack = true;
                    // Enable healthbar items for attack
                    healthBar.gameObject.SetActive(true);
                    timeRemaining.gameObject.SetActive(true);
                    attackNameText.gameObject.SetActive(true);
                    currentAttack = Instantiate(currentAttack);
                    UpdateNameTextbox();
                    healthBar.fillAmount = 1;
                    return;
                }

                timeElapsed += Time.deltaTime;
                return;
            }

            // Handle transitions if the player has timed out the enemy or killed it.
            if (timeElapsed > currentAttack.durationSeconds || damageTaken >= currentAttack.health)
            {
                // Dispose of the current attack's emitter(s).
                currentAttack.CleanUp();

                attackIndex += 1;

                // If we've reached the end of the attack list, handle next actions based on the type of enemy.
                if (attackIndex >= attacks.Length)
                {
                    // If the enemy should loop attacks, return to the beginning of the list.
                    if (shouldLoopAttacks)
                    {
                        attackIndex = 0;
                    }

                    // If the enemy should die at the end of its attack cycle, kill it.
                    if (canBeTimedOut)
                    {
                        vitality.health = 0;
                        vitality.Die();
                        // Disable healthbar items after last attack concludes
                        healthBar.gameObject.SetActive(false);
                        timeRemaining.gameObject.SetActive(false);
                        attackNameText.gameObject.SetActive(false);
                        return;
                    }

                    // The enemy has no more attacks if we've reached the end of the list and shouldn't loop or die.
                    hasAttacksToExecute = false;
                    // Disable healthbar items after last attack concludes
                    healthBar.gameObject.SetActive(false);
                    timeRemaining.gameObject.SetActive(false);
                    attackNameText.gameObject.SetActive(false);
                    return;
                }

                // Prepare for the next attack.
                currentAttack = attacks[attackIndex].GetComponent<EnemyAttack>();
                baselineHealth = vitality.Health;
                damageTaken = 0;
                timeElapsed = 0f;
                inAttack = false;
                // Disable healthbar items between attacks
                healthBar.gameObject.SetActive(false);
                timeRemaining.gameObject.SetActive(false);
                attackNameText.gameObject.SetActive(false);
                return;
            }

            // Update the player's progress.
            timeElapsed += Time.deltaTime;
            timeRemaining.text = $"{(int)(currentAttack.durationSeconds - timeElapsed)}";
            damageTaken = baselineHealth - vitality.Health;
            healthBar.fillAmount = (currentAttack.health - damageTaken)/(float)currentAttack.health;
        }

        private void LateUpdate()
        {

        }

        /// <summary>
        /// Populates the attack name text box when applicable
        /// </summary>
        private void UpdateNameTextbox()
        {
            string updateText = string.IsNullOrEmpty(currentAttack.attackName) ? "" : currentAttack.attackName;
            if (attackNameText != null)
            {
                attackNameText.text = updateText;
            }
        }
    }
}
