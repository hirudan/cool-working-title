using UnityEngine;

namespace BulletManagement
{
    /// <summary>
    /// Base class for bullets. Bullets are destroyed when they no longer are in the view of the Camera.
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        public float decayTime = 1f;
        public float instantiationTime;

        // Metadata
        public Emitter emitter;
        public BulletPattern bulletPattern;
        private int bulletId;

        private void Start()
        {
            // Destroy self in N seconds after emitt
            // Do not do so if set to zero.
            if (decayTime != 0)
            {
                Destroy(gameObject, decayTime);
            }

            instantiationTime = Time.timeSinceLevelLoad;
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        void Update()
        {
            var timeAlive = (Time.timeSinceLevelLoad - instantiationTime);

            // Move in a fixed direction until otherwise
            // Optimize this call if need be when we hit optimization issues.
            Vector3 translation = bulletPattern.GetTranslation(timeAlive, bulletId) * Time.deltaTime;

            // This is to support bullet time, slows down bullet by multiplier
            translation.Scale(emitter.bulletSpeedMultiplier);

            // Move the actual bullet.
            transform.Translate(translation);
        }

        public virtual void SetData(Emitter emitter, BulletPattern bulletPattern, int bulletId)
        {
            this.emitter = emitter;
            this.bulletPattern = bulletPattern;
            this.bulletId = bulletId;
        }
    }
}
