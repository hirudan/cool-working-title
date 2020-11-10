using UnityEngine;

namespace BulletManagement
{
    /**
    * Defines bullet patterns for each bullet.
    * Requires a time t, representing how long a bullet has lived.
    */
    public class BulletPattern : MonoBehaviour
    {
        /// <summary>
        /// Calculate offset from the emitter.
        /// </summary>
        /// <param name="bulletId">Unique bullet ID assigned for each ejected group of bullets.</param>
        /// <returns>Initial position of the bullet for calculating off-set.</returns>
        public virtual Vector3 GetInitialPosition(int bulletId)
        {
            return Vector3.zero;
        }

        /// <summary>
        /// Calculates translation when given time t of the lifetime
        // of an object from float. bulletId is used if each bullet
        // needs to have unique patterns.
        /// </summary>
        /// <param name="time">time in double precision. Usually Time.deltaTime.</param>
        /// <param name="bulletId">Unique bullet ID assigned for each ejected group of bullets.</param>
        /// <returns>Translation vector at time t.</returns>
        public virtual Vector3 GetTranslation(double time, int bulletId)
        {
            return new Vector3((float) time, 0, 0);
        }
    }
}
