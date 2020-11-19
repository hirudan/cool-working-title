using Movement;
using SlipTime;
using UnityEngine;

namespace Pickups
{
    public class Pickup : MonoBehaviour
    {
        /// <summary>
        /// The health threshold at which this pickup should be dropped.
        /// A negative values indicates that this is a time-based pickup.
        /// </summary>
        public int healthThreshold = -1;

        /// <summary>
        /// The time frequency at which this pickup should be dropped.
        /// A negative value indicates that this is a health-based pickup.
        /// </summary>
        public int frequency = -1;

        /// <summary>
        /// Whether this pickup should be continually dropped at the given frequency.
        /// Only applicable for time-based pickups.
        /// </summary>
        public bool shouldLoop;
    }
}
