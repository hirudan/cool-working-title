using System;
using SlipTime;
using UnityEngine;

 namespace BulletManagement
{
    /// <summary>
    /// Emits Bullet types. Each Emitter dictates the bullet speed.
    /// Each bullet is responsible for knowing what speed it should go
    /// as each GameObject must be updated by Unity so this is the most effective.
    /// </summary>
    public abstract class EmitterBase : MonoBehaviour
    {
        // Will attempt to get BulletPattern on object at program start.
        protected BulletPattern bulletPattern;
        // Vector of three Euler angles for rotating the emitter
        private Vector3 rotationVector = new Vector3(0,0,0);

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

        /// <summary>
        /// Generates bullets. Pattern is defined by bulletPattern
        /// </summary>
        protected void EmitBullets()
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

        protected float AngleToPlayer()
        {
            var playerTransform = GameObject.Find("Player").transform;
            var emitterTransform = transform;
            Vector3 targetDir = playerTransform.position - emitterTransform.position;
            return Vector3.SignedAngle(targetDir, emitterTransform.up, Vector3.forward);
        }

        private void Start()
        {
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

        // Update is called once per frame
        void Update()
        {
            if (aimed)
            {
                var angleToPlayer = AngleToPlayer();
                transform.Rotate(Vector3.forward * -angleToPlayer, Space.Self);
            }
            // Apply translation to this emitter
            this.transform.Rotate(Vector3.forward * (spinRate * Time.deltaTime * SlipTimeCoefficient));
            
            timeCounter += Time.deltaTime * bulletSpeedMultiplier * SlipTimeCoefficient;
            if (timeCounter >= emitFrequency)
            {
                EmitBullets();
                timeCounter = 0f;
            }
            
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
