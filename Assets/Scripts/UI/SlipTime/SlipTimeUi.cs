using SlipTime;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SlipTime
{
    /// <summary>
    /// Class that handles the SlipTime UI.
    /// </summary>
    public class SlipTimeUi : MonoBehaviour
    {
        /// <summary>
        /// The SlipTime manager
        /// </summary>
        public SlipTimeManager slipTimeManager;

        /// <summary>
        /// The SlipTime notification image
        /// </summary>
        public Image slipTimeNotification;
        
        /// <summary>
        /// The fill-able image that displays the current duration of SlipTime
        /// </summary>
        public Image durationBar;

        /// <summary>
        /// The fill-able image that displays SlipTimeCharge progress
        /// </summary>
        public Image meter;
        
        /// <summary>
        /// The fill-able images that display how many charges of SlipTime are available
        /// </summary>
        public Image[] bars;
        
        // The opacities to toggle between for the SlipTime notification
        private readonly Color lowOpacity = new Color { a = 0.10f, b = 255f, g = 255f, r = 255f };
        private readonly Color highOpacity = new Color { a = 0.50f, b = 255f, g = 255f, r = 255f };
        
        private int slipTimeCharges;

        // Start is called before the first frame update
        private void Start()
        {
            // Handle image setup
            slipTimeNotification.color = slipTimeManager.InSlipTime ? Color.white : Color.clear;
            
            durationBar.enabled = true;
            durationBar.type = Image.Type.Filled;
            durationBar.fillAmount = 1f;
            
            meter.type = Image.Type.Filled;
            meter.fillAmount = 1f;
            
            foreach (Image img in bars)
            {
                img.type = Image.Type.Filled;
                img.fillAmount = 1f;
            }
            
            slipTimeCharges = slipTimeManager.slipTimeCharges;
        }

        // LateUpdate is guaranteed to be called after Update. We use this for redrawing our charge meter 
        // so that we are sure we have the latest data from SlipTimeManager
        private void LateUpdate()
        {
            // Updates for the SlipTime notification
            slipTimeNotification.color = slipTimeManager.InSlipTime
                ? Color.Lerp(lowOpacity, highOpacity, Mathf.PingPong(Time.time, 0.75f))
                : Color.clear;
            
            // Updates for the duration bar
            durationBar.fillAmount = slipTimeManager.SlipTimeCounter / slipTimeManager.slipTimeDuration;
            
            // Updates for the bars that show number of available charges
            if (slipTimeCharges != slipTimeManager.slipTimeCharges)
            {
                slipTimeCharges = slipTimeManager.slipTimeCharges;
                // Reset bars to unfilled
                foreach (Image img in bars)
                {
                    img.fillAmount = 0;
                }
                // Fill only the bars we need
                for(var i = 0; i < slipTimeCharges; i++)
                {
                    bars[i].fillAmount = 1;
                }
            }

            // Updates for the bar that shows charge progress.
            // SlipTime meter should get emptied when SlipTime is engaged.
            // If SlipTime is being charged, however, continue displaying the charge amount.
            // In the case where we use our first SlipTime charge, the meter would already be full, so we need to empty it.
            if (slipTimeManager.IsChargingSlipTime)
            {
                meter.fillAmount = (slipTimeManager.slipTimeCooldownDuration - slipTimeManager.ChargeTimeCounter) / slipTimeManager.slipTimeCooldownDuration;
            }
            else if (slipTimeManager.InSlipTime)
            {
                meter.fillAmount = 0;
            }
        }
    }
}
