using SlipTime;
using UnityEngine;

namespace BulletManagement
{
    /// <inheritdoc cref="BulletManagement.EmitterBase" />
    public class SlipTimeEmitter : EmitterBase, ISlipTimeAdherent
    {
        public SlipTimeManager SlipTimeManager => slipTimeManager;

        [SerializeField]
        private SlipTimeManager slipTimeManager;
        
        // Update is called once per frame
        private void Update()
        {
            timeCounter += Time.deltaTime * bulletSpeedMultiplier * SlipTimeManager.slipTimeCoefficient;
            if (timeCounter >= emitFrequency)
            {
                EmitBullets();
                timeCounter = 0f;
            }
        }
    }
}
