using UnityEngine;

namespace BulletManagement
{
    /// <summary>
    /// Defines bullet patterns for each bullet.
    /// Requires a time t, representing how long a bullet has lived.
    /// </summary>
    public class BulletPattern : MonoBehaviour
    {
        /// <summary>
        /// Calculates translation when given time t of the lifetime of an object from float.
        /// </summary>
        /// <param name="time">The time of the lifetime of the object.</param>
        /// <param name="bulletId">The unique ID of a bullet. Used if each bullet needs to have unique patterns.</param>
        /// <returns></returns>
        public virtual Vector3 GetTranslation(float time, int bulletId)
        {
            return new Vector3(time, 0, 0);
        }
    }
}
