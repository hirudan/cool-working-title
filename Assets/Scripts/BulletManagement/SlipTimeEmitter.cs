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
        
        // Running total of emitted bullet waves
        private int cyclesEmitted = 0;

        private bool limited = false;

        public float bulletSpeedMultiplier = 1.0f;
        
        // The number of bullet waves to emit. Set to zero for no limit.
        public int emitCycles = 0;

        // Number of bullets to emit each emit cycle.
        public int emitBulletCount;
        
        // Angle at which the emtiter points
        public float startAngle = 0f;

        // The rate at which the emitter should spin each update cycle
        public float spinRate = 0f;

        // The maximum spin rate at which spin will reverse
        public float spinMax = 1f;
        
        // Analogous to angular acceleration, the rate at which spinRate increases
        public float spinAccel = 0f;
        
        // Flag used to control if the spin should reverse upon hitting max speed
        public bool reverseSpin = false;
        
        // Determines if the emitter should aim at the player or not. Note that this will overwrite startAngle.
        public bool aimed = false;

        void Start()
        {
            limited = emitCycles != 0;
            this.bulletPattern = this.GetComponent<BulletPattern>();
            if (aimed)
            {
                var angleToPlayer = AngleToPlayer();
                transform.Rotate(Vector3.forward * -angleToPlayer, Space.Self);
            }
            else
            {
                transform.Rotate(Vector3.forward * startAngle, Space.Self);
            }

        }
        
        protected float AngleToPlayer()
        {
            var playerTransform = GameObject.Find("Player").transform;
            var emitterTransform = transform;
            Vector3 targetDir = playerTransform.position - emitterTransform.position;
            return Vector3.SignedAngle(targetDir, emitterTransform.up, Vector3.forward);
        }

        private void EmitBullets()
        {
            // Instantiate the bulletPrefab at emitter position
            for (int id = 0; id < this.emitBulletCount; ++id)
            {
                var currentTransform = this.transform;
                GameObject bulletGO = Instantiate(bulletPrefab, currentTransform.position, currentTransform.rotation);
                Bullet bullet = bulletGO.GetComponent<Bullet>();
                bullet.SetData(this, this.bulletPattern, id);
                bullet.decayTime = bulletDecayTime;
            }
        }
        
        // Update is called once per frame
        private void Update()
        {
            // Destroy the emitter if it has finished all its cycles (when applicable)
            if(limited && cyclesEmitted >= emitCycles) Destroy(this);
            if (aimed)
            {
                var angleToPlayer = AngleToPlayer();
                transform.Rotate(Vector3.forward * -angleToPlayer, Space.Self);
            }
            // Apply translation to this emitter
            transform.Rotate(Vector3.forward * (spinRate * Time.deltaTime * SlipTimeManager.slipTimeCoefficient));
            
            timeCounter += Time.deltaTime * bulletSpeedMultiplier * SlipTimeManager.slipTimeCoefficient;
            if (!(timeCounter >= emitFrequency) || limited && (cyclesEmitted >= emitCycles)) return;
            EmitBullets();
            timeCounter = 0f;
            cyclesEmitted++;
            
            // Update spin rate
            spinRate += spinAccel; 

            //Invert the spin if set to 1
            if (!reverseSpin) return;
            if (spinRate < -spinMax || spinRate > spinMax) {
                spinAccel = -spinAccel;
            }
        }
    }
}
