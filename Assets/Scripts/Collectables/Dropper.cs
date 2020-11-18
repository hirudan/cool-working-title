using UnityEngine;

namespace Collectables
{
    /// <summary>
    /// A generic dropper that drops collectables (drops).
    /// </summary>
    public abstract class Dropper : MonoBehaviour
    {
        /// <summary>
        /// The collectable to drop.
        /// </summary>
        public GameObject dropPrefab;
    }
}
