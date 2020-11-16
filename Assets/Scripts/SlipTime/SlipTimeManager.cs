using UnityEngine;

namespace SlipTime
{
    /// <summary>
    /// Manages activating and deactivating SlipTime.
    /// </summary>
    public class SlipTimeManager : MonoBehaviour
    {
        /// <summary>
        /// The percentage by which to scale time in SlipTime.
        /// </summary>
        public float slipTimeScalar = 0.25f;

        /// <summary>
        /// The coefficient by which to multiply all time calculations.
        /// </summary>
        public float slipTimeCoefficient = 1f;

        /// <summary>
        /// The default duration of SlipTime in seconds.
        /// </summary>
        public float slipTimeDuration = 7f;

        /// <summary>
        /// The maximum number of charges of SlipTime a player can have.
        /// </summary>
        public int maxSlipTimeCharges = 3;

        /// <summary>
        /// The number of SlipTime charges the player currently has.
        /// </summary>
        /// <returns></returns>
        public int slipTimeCharges;

        /// <summary>
        /// The default amount of time it takes to charge one charge of SlipTime.
        /// </summary>
        public float slipTimeCooldownDuration = 60f;

        /// <summary>
        /// How much SlipTime duration to subtract if the player moves during SlipTime.
        /// </summary>
        public float slipTimeMovementPenaltyCoefficient = 0.5f;

        /// <summary>
        /// The default amount of time to decrease the current cooldown of SlipTime by when bullet grazing.
        /// </summary>
        public float bulletGrazingCooldownDecreaseCoefficient = 0.5f;

        /// <summary>
        /// Counter to keep track of time spent in SlipTime.
        /// </summary>
        public float SlipTimeCounter { get; private set; }

        /// <summary>
        /// Counter to keep track of time spent charging SlipTime.
        /// </summary>
        public float ChargeTimeCounter { get; private set; }

        /// <summary>
        /// Whether the player is in SlipTime.
        /// </summary>
        public bool InSlipTime { get; private set; }

        /// <summary>
        /// Whether the player is charging a charge of SlipTime.
        /// </summary>
        public bool IsChargingSlipTime { get; private set; }

        private void Start()
        {
            slipTimeCharges = maxSlipTimeCharges;
            InSlipTime = false;
            IsChargingSlipTime = false;
            SlipTimeCounter = slipTimeDuration;
            ChargeTimeCounter = slipTimeCooldownDuration;
        }

        // Update is called once per frame.
        private void Update()
        {
            // Logic for entering SlipTime.
            if (Input.GetButtonDown("Fire2") && !InSlipTime && slipTimeCharges > 0)
            {
                InSlipTime = true;
                slipTimeCoefficient = slipTimeScalar;
                slipTimeCharges -= 1;
            }

            // Logic for exiting SlipTime at user prompt.
            else if (Input.GetButtonDown("Fire2") && InSlipTime)
            {
                InSlipTime = false;
                SlipTimeCounter = slipTimeDuration;
                slipTimeCoefficient = 1f;
                IsChargingSlipTime = true;
            }

            // Logic to decrease the active SlipTime duration if the player moves.
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical") && InSlipTime)
            {
                SlipTimeCounter -= Time.deltaTime * slipTimeMovementPenaltyCoefficient;
            }

            // Logic to keep track of when to exit SlipTime by default.
            if (InSlipTime)
            {
                SlipTimeCounter -= Time.deltaTime;
                if (SlipTimeCounter <= 0)
                {
                    InSlipTime = false;
                    SlipTimeCounter = slipTimeDuration;
                    slipTimeCoefficient = 1f;
                    IsChargingSlipTime = true;
                }
            }

            // Logic to handle the cooldown of SlipTime charges.
            if (IsChargingSlipTime)
            {
                ChargeTimeCounter -= Time.deltaTime;
                if (ChargeTimeCounter <= 0)
                {
                    slipTimeCharges += 1;
                    ChargeTimeCounter = slipTimeCooldownDuration;
                    if (slipTimeCharges == maxSlipTimeCharges)
                    {
                        IsChargingSlipTime = false;
                    }
                }
            }
        }

        /// <summary>
        /// Decrease the current cooldown of SlipTime.
        /// </summary>
        public void DecreaseCooldown()
        {
            if (slipTimeCharges < maxSlipTimeCharges)
            {
                ChargeTimeCounter -= Time.deltaTime * bulletGrazingCooldownDecreaseCoefficient;
            }
        }
    }
}
