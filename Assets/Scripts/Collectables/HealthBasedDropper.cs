using System.Collections.Generic;
using System.Linq;
using Actor;
using UnityEngine;

namespace Collectables
{
    /// <summary>
    /// A dropper that drops drops based on the health of the entity it's bound to.
    /// </summary>
    public class HealthBasedDropper : Dropper
    {
        /// <summary>
        /// The entity that this dropper is attached to. Used to keep track of health.
        /// </summary>
        public Living entity;
        
        /// <summary>
        /// The health points at which to drop drops.
        /// </summary>
        public int[] healthMarkers;

        private IList<int> sortedHealthMarkers;

        private void Start()
        {
            // If there are no health markers for which to drop drops, destroy this object.
            if (healthMarkers.Length <= 0)
            {
                Destroy(this);
            }

            // Ensure that the health markers are sorted in descending order and convert to a list for easier manipulation.
            sortedHealthMarkers = new List<int>(healthMarkers.OrderByDescending(x => x).ToArray());
        }

        private void Update()
        {
            if (entity.Health <= sortedHealthMarkers[0])
            {
                Transform currentTransform = transform;
                Instantiate(dropPrefab, currentTransform.position, currentTransform.rotation);

                sortedHealthMarkers.RemoveAt(0);
                if (sortedHealthMarkers.Count <= 0)
                {
                    Destroy(this);
                }
            }
        }
    }
}
