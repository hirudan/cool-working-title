using SlipTime;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SlipTimeUi : MonoBehaviour
    {
        public SlipTimeManager slipTimeManager;
    
        public Text slipTimeUiText;

        private int slipTimeCharges;

        // Start is called before the first frame update
        void Start()
        {
            slipTimeCharges = slipTimeManager.slipTimeCharges;
            slipTimeUiText.text = $"Slip Time Charges: {slipTimeCharges}";
        }

        // Update is called once per frame
        void Update()
        {
            if (slipTimeCharges != slipTimeManager.slipTimeCharges)
            {
                slipTimeCharges = slipTimeManager.slipTimeCharges;
                slipTimeUiText.text = $"Slip Time Charges: {slipTimeCharges}";
            }
        }
    }
}
