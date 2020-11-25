using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Health
{
    public class HealthUi : MonoBehaviour
    {
        public Image healthBlocker;
        private int discreteCount = 3;

        public void Start()
        {
            // We track the blocking of the discrete images
            healthBlocker.type = Image.Type.Filled;
            healthBlocker.fillAmount = 0f;
        }

        public void setUIState(float curHealth, float totalHealth)
        {
            // We calculate values flipped until the end so we reduce
            // rounding error
            float invertedThreshold = (float) discreteCount;
            float invertedPercentage = totalHealth /curHealth;

            // Round downwards since health is discrete
            healthBlocker.fillAmount = 1f - (invertedThreshold / invertedPercentage)  / discreteCount;
        }
    }
}
