using SlipTime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class SlipTimeUI : MonoBehaviour
    {
        public SlipTimeManager slipTimeManager;
    
        public Text slipTimeUIText;

        private int slipTimeCharges;

        // Start is called before the first frame update
        void Start()
        {
            slipTimeCharges = slipTimeManager.slipTimeCharges;
            slipTimeUIText.text = $"Slip Time Charges: {slipTimeCharges}";
        }

        // Update is called once per frame
        void Update()
        {
            if (slipTimeCharges != slipTimeManager.slipTimeCharges)
            {
                slipTimeCharges = slipTimeManager.slipTimeCharges;
                slipTimeUIText.text = $"Slip Time Charges: {slipTimeCharges}";
            }
        }
    }
}
