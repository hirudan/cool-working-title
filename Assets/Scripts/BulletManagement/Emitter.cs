using SlipTime;
using UnityEngine;

namespace BulletManagement
{
    /**
    * Emits Bullet types. Each Emitter dictates the bullet speed.
    * Each bullet is responsible for knowing what speed it should go
    * as each GameObject must be updated by Unity so this is the most effective.
    */
    public class Emitter : SlipTimeAdherent
    {
        // Will attempt to get BulletPattern on object at program start.
        private BulletPattern bulletPattern;

        public float bulletSpeedMultiplier = 1.0f;
        
        public GameObject bulletPrefab;

        public double timeCounter = 0.0;

        // Number of bullets to emit each emit cycle.
        public int emitBulletCount;

        // Number of seconds to wait until next emit
        // set to 0 to emit on instantiation.
        public float emitFrequency;

        // Bullet decay, can be set to 0f for no decay
        public float bulletDecayTime = 0f;

        // Generates bullets
        // Pattern is defined by bulletPattern
        protected void EmitBullets() {
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
            this.SlipTimeCoefficient = 1.0f;
            this.bulletPattern = this.GetComponent<BulletPattern>();
        }

        // Update is called once per frame
        void Update()
        {
            timeCounter += Time.deltaTime * bulletSpeedMultiplier * SlipTimeCoefficient;
            if (timeCounter >= emitFrequency)
            {
                EmitBullets();
                timeCounter = 0f;
            }
        }
    }
}
