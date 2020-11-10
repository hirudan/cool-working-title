using UnityEngine;

namespace BulletManagement
{
    // An emitter that emits a fixed number of bullet cycles and then destroys itself
    public class LimitedEmitter : Emitter
    {
        // The number of bullet waves to emit
        public int emitCycles = 1;

        // Running total of emitted bullet waves
        private int _cyclesEmitted = 0;
        
        // Update is called once per frame
        void Update()
        {
            if(_cyclesEmitted >= emitCycles) Destroy(this);
            timeCounter += Time.deltaTime * bulletSpeedMultiplier;
            if (!(timeCounter >= emitFrequency) || _cyclesEmitted >= emitCycles) return;
            EmitBullets();
            timeCounter = 0f;
            _cyclesEmitted++;
        }
    }
}