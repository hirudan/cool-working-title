using UnityEngine;

namespace BulletManagement
{
    /**
    * Base class for bullets. Bullets are destroyed when they no longer are
    * in the view of the Camera.
    */
    public class Bullet : MonoBehaviour
    {
        public float decayTime = 1f;
        public double timeAlive = 0.0;

        // Metadata
        public Emitter emitter = null;
        public BulletPattern bulletPattern = null;

        [SerializeField]
        private int bulletId;

        // Getter and Setters
        public int BulletId 
        {
            get { return this.bulletId; } 
            private set { this.bulletId = value; }
        }

        void OnBecameInvisible() {
            Destroy(this.gameObject);
        }

        void Start()
        {
            transform.Translate(bulletPattern.GetInitialPosition(this.bulletId));
        }

        void Update()
        {
            // When speeding up bullet, the input needs to be multipled and also the derivative is multiplied.
            timeAlive += Time.deltaTime * emitter.bulletSpeedMultiplier;

            // Need to keep track of time so that slo-motion will effect decay as well.
            if (decayTime != 0 && timeAlive >= decayTime)
            {
                Destroy(this.gameObject);
            }

            // Move in a fixed direction until otherwise
            // Optimize this call if need be when we hit optimization issues.
            Vector3 translation = this.bulletPattern.GetTranslation(timeAlive, this.bulletId) * Time.deltaTime;

            // Move the actual bullet.
            transform.Translate(translation * emitter.bulletSpeedMultiplier);
        }

        public virtual void SetData(Emitter emitter, BulletPattern bulletPattern, int bulletId)
        {
            this.emitter = emitter;
            this.bulletPattern = bulletPattern;
            this.bulletId = bulletId;
        }
    }
}
