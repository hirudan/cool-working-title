using UnityEngine;

namespace BulletManagement
{
    /**
    * Emits Bullet types. Each Emitter dictates the bullet speed.
    * Each bullet is responsible for knowing what speed it should go
    * as each GameObject must be updated by Unity so this is the most effective.
    */
    public class Emitter : MonoBehaviour
    {
        // Will attempt to get BulletPattern on object at program start.
        private BulletPattern bulletPattern;

        // We encode speed as a Vector so that some bullets are faster
        // one way than another.
        public Vector3 bulletSpeedMultiplier = new Vector3(1, 1, 1);
        public GameObject bulletPrefab;

        // Number of bullets to emit each emit cycle.
        public int emitBulletCount;

        // Number of seconds to wait until next emit
        // set to 0 to emit on instantiation.
        public float emitFrequency;

        // Bullet decay, can be set to 0f for no decay
        public float bulletDecayTime = 0f;

        // Generates bullets
        // Pattern is defined by bulletPattern
        void EmitBullets() {
            // Instantiate the bulletPrefab at emitter position
            for (int id = 0; id < this.emitBulletCount; ++id)
            {
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Bullet bullet = bulletGO.GetComponent<Bullet>();
                bullet.SetData(this, this.bulletPattern, id);
                bullet.decayTime = bulletDecayTime;
            }
        }

        void Start()
        {
            this.bulletPattern = this.GetComponent<BulletPattern>();

            if (emitFrequency != 0)
            {
                InvokeRepeating("EmitBullets", this.emitFrequency, this.emitFrequency);
            }
            EmitBullets();
        }
    }
}
