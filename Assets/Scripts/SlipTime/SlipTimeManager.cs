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
        /// The default duration of SlipTime in seconds.
        /// </summary>
        public float slipTimeDuration = 3f;
        
        private bool inSlipTime;
        private SlipTimeAdherent[] slipTimeAdherents;
        private double timeCounter;

        // Update is called once per frame
        private void Start()
        {
            slipTimeAdherents = FindObjectsOfType<SlipTimeAdherent>();
        }

        private void Update()
        {
            // Logic for entering SlipTime
            if (Input.GetButtonDown("Fire2") && !inSlipTime)
            {
                inSlipTime = true;
                foreach (SlipTimeAdherent slipTimeAdherent in slipTimeAdherents)
                {
                    slipTimeAdherent.SlipTimeCoefficient = slipTimeScalar;
                }
            }
            
            // Logic for exiting SlipTime at user prompt
            else if (Input.GetButtonDown("Fire2") && inSlipTime)
            {
                inSlipTime = false;
                foreach (SlipTimeAdherent slipTimeAdherent in slipTimeAdherents)
                {
                    slipTimeAdherent.SlipTimeCoefficient = 1f;
                }
            }
            
            // Logic to keep track of when to exit SlipTime by default
            if (inSlipTime)
            {
                timeCounter += Time.deltaTime;
                if (timeCounter > slipTimeDuration)
                {
                    inSlipTime = false;
                    timeCounter = 0f;
                    foreach (SlipTimeAdherent slipTimeAdherent in slipTimeAdherents)
                    {
                        slipTimeAdherent.SlipTimeCoefficient = 1f;
                    }
                }
            }
        }
    }
}
