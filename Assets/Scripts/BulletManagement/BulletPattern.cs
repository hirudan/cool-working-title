using UnityEngine;

/**
 * Defines bullet patterns for each bullet.
 * Requires a time t, representing how long a bullet has lived.
 * Encodes bullet movement as delta T.
 */
public class BulletPattern : MonoBehaviour
{
    // Calculates the displacement when given time t of the lifetime
    // of an object from float. bulletId is used if each bullet
    // needs to have unique patterns.
    public virtual Vector3 GetTranslation(float time, int bulletId)
    {
        return new Vector3(time, 0, 0);
    }
}
