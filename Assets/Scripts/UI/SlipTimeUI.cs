using SlipTime;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SlipTimeUI : MonoBehaviour
    {
        public SlipTimeManager slipTimeManager;

        private int slipTimeCharges;

        // Fillable images for the sliptime meter and charge bars
        public Image meter;
        public Image[] bars;

        // Start is called before the first frame update
        private void Start()
        {
            // Handle image setup
            meter.type = Image.Type.Filled;
            meter.fillAmount = 1;
            foreach (var img in bars)
            {
                img.type = Image.Type.Filled;
                img.fillAmount = 1;
            }
            slipTimeCharges = slipTimeManager.slipTimeCharges;
        }

        // LateUpdate is guaranteed to be called after Update. We use this for redrawing our charge meter 
        // so that we are sure we have the latest data from SliptimeManager
        private void LateUpdate()
        {
            if (slipTimeCharges != slipTimeManager.slipTimeCharges)
            {
                slipTimeCharges = slipTimeManager.slipTimeCharges;
                // Reset bars to unfilled
                foreach (var img in bars)
                {
                    img.fillAmount = 0;
                }
                // Fill only the bars we need
                for(var i = 0; i < slipTimeCharges; i++)
                {
                    bars[i].fillAmount = 1;
                }
            }

            // Sliptime meter should get emptied when sliptime is engaged. If sliptime is being charged, however, continue displaying the charge 
            // amount. In the case where we use our first sliptime charge, the meter would already be full, so we need to empty it.
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
