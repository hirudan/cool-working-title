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

        public float bulletSpeedMultiplier = 1.0f;

        public GameObject bulletPrefab;
        public GameObject owner;

        protected double timeCounter = 0.0;

        // Number of bullets to emit each emit cycle.
        public int emitBulletCount;

        // Number of seconds to wait until next emit
        // set to 0 to emit on instantiation.
        public float emitFrequency;

        // Bullet decay, can be set to 0f for no decay
        public float bulletDecayTime = 0f;

        [SerializeField]
        public string ownerName;

        protected void Start()
        {
            this.bulletPattern = this.GetComponent<BulletPattern>();
            if (transform.parent)
            {
                ownerName = transform.parent.name;
            }
            else if(owner)
            {
                ownerName = owner.name;
            }
            else
            {
                ownerName = "ROOT";
            }
        }

        /// <summary>
        /// Generates bullets. Pattern is defined by bulletPattern
        /// </summary>
        protected void EmitBullets()
        {
            // Instantiate the bulletPrefab at emitter position
            for (var id = 0; id < this.emitBulletCount; ++id)
            {
                Transform currentTransform = this.transform;
                GameObject bulletGO = Instantiate(bulletPrefab, currentTransform.position, currentTransform.rotation);
                var bullet = bulletGO.GetComponent<Bullet>();
                bullet.SetData(ownerName, this, this.bulletPattern, id);
                bullet.decayTime = bulletDecayTime;
            }
        }
    }
}
