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
        public float slipTimeDuration = 3f;

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
        public float slipTimeChargeDuration = 3;

        private bool inSlipTime;
        private double slipTimeCounter;
        private double chargeTimeCounter;

        private void Start()
        {
            slipTimeCharges = maxSlipTimeCharges;
        }

        // Update is called once per frame
        private void Update()
        {
            // Logic for entering SlipTime
            if (Input.GetButtonDown("Fire2") && !inSlipTime && slipTimeCharges > 0)
            {
                inSlipTime = true;
                slipTimeCoefficient = slipTimeScalar;
                slipTimeCharges -= 1;
            }

            // Logic for exiting SlipTime at user prompt
            else if (Input.GetButtonDown("Fire2") && inSlipTime)
            {
                inSlipTime = false;
                slipTimeCounter = 0f;
                slipTimeCoefficient = 1f;
            }

            // Logic to keep track of when to exit SlipTime by default
            if (inSlipTime)
            {
                slipTimeCounter += Time.deltaTime;
                if (slipTimeCounter > slipTimeDuration)
                {
                    inSlipTime = false;
                    slipTimeCounter = 0f;
                    slipTimeCoefficient = 1f;
                }
            }

            if (slipTimeCharges < maxSlipTimeCharges && !inSlipTime)
            {
                chargeTimeCounter += Time.deltaTime;
                if (chargeTimeCounter >= slipTimeChargeDuration)
                {
                    slipTimeCharges += 1;
                    chargeTimeCounter = 0f;
                }
            }
        }
    }
}
