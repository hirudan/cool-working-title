using UnityEngine;

/**
 * Base class for bullets. Bullets are destroyed when they no longer are
 * in the view of the Camera.
 */
public class Bullet : MonoBehaviour
{
    public float decayTime = 1f;
    public float instantiationTime = 0.0f;

    // Metadata
    public Emitter emitter = null;
    public BulletPattern bulletPattern = null;
    private int bulletId;

    void Start()
    {
        // Destroy self in N seconds after emitt
        // Do not do so if set to zero.
        if (decayTime != 0)
        {
            Destroy(this.gameObject, decayTime);
        }
        this.instantiationTime = Time.timeSinceLevelLoad;
    }

    void OnBecameInvisible() {
        Destroy(this.gameObject);
    }

    void Update()
    {
        float timeAlive = (Time.timeSinceLevelLoad - this.instantiationTime);

        // Move in a fixed direction until otherwise
        // Optimize this call if need be when we hit optimization issues.
        Vector3 translation = this.bulletPattern.GetTranslation(timeAlive, this.bulletId) * Time.deltaTime;

        // This is to support bullet time, slows down bullet by multiplier
        translation.Scale(this.emitter.bulletSpeedMultiplier);

        // Move the actual bullet.
        transform.Translate(translation);
    }

    public virtual void SetData(Emitter emitter, BulletPattern bulletPattern, int bulletId)
    {
        this.emitter = emitter;
        this.bulletPattern = bulletPattern;
        this.bulletId = bulletId;
    }

    public int getBulletId()
    {
        return this.bulletId;
    }
}
