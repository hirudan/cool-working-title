using UnityEngine;

namespace BulletManagement
{
    /**
    * Defines bullet patterns for each bullet.
    * Requires a time t, representing how long a bullet has lived.
    */
    public class BulletPattern : MonoBehaviour
    {
        // Calculates translation when given time t of the lifetime
        // of an object from float. bulletId is used if each bullet
        // needs to have unique patterns.
        public virtual Vector3 GetTranslation(float time, int bulletId)
        {
            return new Vector3(time, 0, 0);
        }
    }
}
