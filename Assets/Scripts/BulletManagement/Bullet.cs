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
        public EmitterBase emitter = null;
        public BulletPattern bulletPattern = null;

        [SerializeField]
        private int bulletId;

        // Getter and Setters
        public int BulletId => this.bulletId;

        void OnBecameInvisible()
        {
            Destroy(this.gameObject);
        }

        void Start()
        {
            transform.Translate(bulletPattern.GetInitialPosition(this.bulletId));
        }

        void Update()
        {
            Vector3 translation;
            
            // When speeding up bullet, the input needs to be multiplied and also the derivative is multiplied.
            // Speed and SlipTime modifiers should all come from the emitter.
            if (emitter is Emitter slipTimeEmitter)
            {
                timeAlive += Time.deltaTime * emitter.bulletSpeedMultiplier * slipTimeEmitter.SlipTimeManager.slipTimeCoefficient;
                
                // Move in a fixed direction until otherwise
                // Optimize this call if need be when we hit optimization issues.
                translation = this.bulletPattern.GetTranslation(timeAlive, this.bulletId) * Time.deltaTime;
                translation *= emitter.bulletSpeedMultiplier * slipTimeEmitter.SlipTimeManager.slipTimeCoefficient;
            }
            else
            {
                timeAlive += Time.deltaTime * emitter.bulletSpeedMultiplier;
                
                // Move in a fixed direction until otherwise
                // Optimize this call if need be when we hit optimization issues.
                translation = this.bulletPattern.GetTranslation(timeAlive, this.bulletId) * Time.deltaTime;
                translation *= emitter.bulletSpeedMultiplier;
            }

            // Need to keep track of time so that slo-motion will effect decay as well.
            if (decayTime != 0 && timeAlive >= decayTime)
            {
                Destroy(this.gameObject);
            }

            // Move the actual bullet. Speed and SlipTime modifiers should all come from the emitter.
            transform.Translate(translation);
        }

        public virtual void SetData(EmitterBase emitter, BulletPattern bulletPattern, int bulletId)
        {
            this.emitter = emitter;
            this.bulletPattern = bulletPattern;
            this.bulletId = bulletId;
        }
    }
}
