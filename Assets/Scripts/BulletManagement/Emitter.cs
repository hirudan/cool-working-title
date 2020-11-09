﻿using UnityEngine;

namespace BulletManagement
{
    /// <summary>
    /// Emits Bullet types. Each Emitter dictates the bullet speed.
    /// Each bullet is responsible for knowing what speed it should go
    /// as each GameObject must be updated by Unity so this is the most effective.
    /// </summary>
    public class Emitter : MonoBehaviour
    {
        // We encode speed as a Vector so that some bullets are faster one way than another.
        public Vector3 bulletSpeedMultiplier = new Vector3(1, 1, 1);
        public BulletPattern bulletPattern;
        public GameObject bulletPrefab;

        // Number of bullets to emit each emit cycle.
        public int emitBulletCount;

        // Number of seconds to wait until next emit
        // set to 0 to emit on instantiation.
        public float emitFrequency;

        // Bullet decay, can be set to 0f for no decay
        public float bulletDecayTime;

        // Generates bullets
        // Pattern is defined by bulletPattern
        private void EmitBullets()
        {
            // Instantiate the bulletPrefab at emitter position
            for (int id = 0; id < emitBulletCount; ++id)
            {
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Bullet bullet = bulletGO.GetComponent<Bullet>();
                bullet.SetData(this, bulletPattern, id);
                bullet.decayTime = bulletDecayTime;
            }
        }

        private void Start()
        {
            if (emitFrequency != 0)
            {
                InvokeRepeating("EmitBullets", emitFrequency, emitFrequency);
            }

            EmitBullets();
        }
    }
}
