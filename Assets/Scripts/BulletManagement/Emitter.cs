using SlipTime;
using UnityEngine;

namespace BulletManagement
{
    /// <inheritdoc cref="BulletManagement.EmitterBase" />
    public class Emitter : EmitterBase, ISlipTimeAdherent
    {
        public SlipTimeManager SlipTimeManager => slipTimeManager;

        [SerializeField]
        private SlipTimeManager slipTimeManager;

        // Generates bullets
        // Pattern is defined by bulletPattern
        protected override void EmitBullets()
        {
            // Instantiate the bulletPrefab at emitter position
            for (int id = 0; id < this.emitBulletCount; ++id)
            {
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Bullet bullet = bulletGO.GetComponent<Bullet>();
                bullet.SetData(this, this.bulletPattern, id);
                bullet.decayTime = bulletDecayTime;
            }
        }

        private void Start()
        {
            this.bulletPattern = this.GetComponent<BulletPattern>();
        }

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
