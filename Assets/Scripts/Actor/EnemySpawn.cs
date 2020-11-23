using UnityEngine;

namespace Actor
{
    public class EnemySpawn
    {
        // The time at which to spawn an enemy in
        public float spawnTime { get; set; }
        
        // A prefab of the enemy to spawn in
        public GameObject enemy { get; set; }
        
        // The location at which to spawn the enemy
        public Vector3 spawnPosition { get; set; }
        
        // Set true if no more enemies should spawn until this one dies
        public bool waitUntilDead { get; set; }
    }
}
