using System;
using SlipTime;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SlipTimeUI : MonoBehaviour
    {
        public SlipTimeManager slipTimeManager;

        public Text slipTimeUIText;

        private int slipTimeCharges;

        // Start is called before the first frame update
        private void Start()
        {
            slipTimeCharges = slipTimeManager.slipTimeCharges;
            slipTimeUIText.text = $"Slip Time Charges: {slipTimeCharges}";
        }

        // Update is called once per frame
        private void Update()
        {
            var slipTimeText = $"Slip Time Charges: {slipTimeManager.slipTimeCharges}";
            
            // Leave this code here for now. We may be able to re-use it later once we get a better UI for SlipTime
            // if we don't have to constantly update the text box.
            // if (slipTimeCharges != slipTimeManager.slipTimeCharges)
            // {
            //     slipTimeCharges = slipTimeManager.slipTimeCharges;
            //     slipTimeUIText.text = $"Slip Time Charges: {slipTimeCharges}";
            // }

            if (slipTimeManager.InSlipTime)
            {
                slipTimeText += $"\nSlip Time Duration: {Convert.ToInt32(slipTimeManager.SlipTimeCounter)}";
            }

            if (slipTimeManager.IsChargingSlipTime)
            {
                slipTimeText += $"\nSlip Time Cooldown: {Convert.ToInt32(slipTimeManager.ChargeTimeCounter)}";
            }

            slipTimeUIText.text = slipTimeText;
        }
    }
}
