using System.Collections.Generic;
using System.Linq;
using Actor;
using Movement;
using SlipTime;
using UnityEngine;

namespace Pickups
{
    /// <summary>
    /// A generic dropper that drops pickups.
    /// </summary>
    public class Dropper : MonoBehaviour, ISlipTimeAdherent
    {
        /// <inheritdoc />
        public SlipTimeManager SlipTimeManager => slipTimeManager;

        /// <summary>
        /// The SlipTimeManager.
        /// </summary>
        [SerializeField]
        private SlipTimeManager slipTimeManager;
        
        /// <summary>
        /// A list of pickup prefabs to drop.
        /// </summary>
        [SerializeField]
        public GameObject[] pickupPrefabs;

        // The entity that this dropper is attached to. Used to keep track of health.
        [SerializeField]
        private Living entity;

        // A list of health-based pickups attached to this dropper.
        private readonly List<Pickup> healthBasedPickups = new List<Pickup>();

        // A list of health-based pickups attached to this dropper sorted by descending health.
        private List<Pickup> sortedHealthBasedPickups;

        // A dictionary of time-based pickups to floats,
        // where the floats are the counters that keep track of when the pickup should be dropped.
        private Dictionary<Pickup, float> timeBasedPickupsAndCounters = new Dictionary<Pickup, float>();

        private void Start()
        {
            entity = gameObject.GetComponent<Living>();

            // Get list of health-based pickups.
            var healthBasedPickupGameObjects = pickupPrefabs.Where(p => p.GetComponent<Pickup>().healthThreshold >= 0);
            foreach (GameObject pickupGameObject in healthBasedPickupGameObjects)
            { 
                pickupGameObject.GetComponent<SlipTimeMover>().SlipTimeManager = slipTimeManager;
                healthBasedPickups.Add(pickupGameObject.GetComponent<Pickup>());
            }
            sortedHealthBasedPickups = healthBasedPickups.OrderByDescending(x => x.healthThreshold).ToList();

            // Get list of time-based pickups.
            var timeBasedPickupGameObjects = pickupPrefabs.Where(p => p.GetComponent<Pickup>().frequency >= 0);
            foreach (GameObject pickupGameObject in timeBasedPickupGameObjects)
            {
                pickupGameObject.GetComponent<SlipTimeMover>().SlipTimeManager = slipTimeManager;
                timeBasedPickupsAndCounters[pickupGameObject.GetComponent<Pickup>()] = 0;
            }
        }

        private void Update()
        {
            // Handle health-based pickups.
            if (sortedHealthBasedPickups.Count > 0)
            {
                if (entity.Health <= sortedHealthBasedPickups[0].healthThreshold)
                {
                    Transform currentTransform = transform;
                    Instantiate(sortedHealthBasedPickups[0].gameObject, currentTransform.position, currentTransform.rotation);

                    sortedHealthBasedPickups.RemoveAt(0);
                }
            }

            // Handle time-based pickups.
            // Create a new dictionary instead of modifying the existing one because modifying the existing one in the loop throws errors.
            var newTimeBasedPickupsAndCounters = new Dictionary<Pickup, float>();
            foreach (var pickupAndCounter in timeBasedPickupsAndCounters)
            {
                Pickup pickup = pickupAndCounter.Key;
                float counter = pickupAndCounter.Value;
                if (counter >= pickup.frequency)
                {
                    Transform currentTransform = transform;
                    Instantiate(pickup.gameObject, currentTransform.position, currentTransform.rotation);
                
                    // If this pickup should be dropped in a loop, reset its counter to 0.
                    if (pickup.shouldLoop)
                    {
                        newTimeBasedPickupsAndCounters[pickup] = 0;
                    }
                }
                else
                {
                    newTimeBasedPickupsAndCounters[pickup] = counter + Time.deltaTime * slipTimeManager.slipTimeCoefficient;
                }
            }

            timeBasedPickupsAndCounters = newTimeBasedPickupsAndCounters;
        }
    }
}
