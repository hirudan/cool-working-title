using SlipTime;
using UnityEngine;

namespace Actor
{
    public class Shield : MonoBehaviour
    {
        public SlipTimeManager slipTimeManager;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == 10)
            {
                slipTimeManager.DecreaseCooldown();
            }
        }
    }
}
