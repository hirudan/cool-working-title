using UnityEngine;

namespace Collectables
{
    /// <summary>
    /// A dropper that drops drops based on time.
    /// </summary>
    public class TimeBasedDropper : Dropper
    {
        /// <summary>
        /// The frequency with which to drop drops.
        /// </summary>
        public int frequency;

        /// <summary>
        /// Whether this dropper should only drop one drop and then destroy itself.
        /// </summary>
        public bool oneTimeDrop;

        private float timeCounter;
        
        private void Update()
        {
            if (timeCounter >= frequency)
            {
                Transform currentTransform = transform;
                Instantiate(dropPrefab, currentTransform.position, currentTransform.rotation);
                if (!oneTimeDrop)
                {
                    timeCounter = 0;
                }
                else
                {
                    Destroy(this);
                }
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }
    }
}
