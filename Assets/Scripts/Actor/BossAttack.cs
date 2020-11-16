using BulletManagement;
using JetBrains.Annotations;
using SlipTime;
using UnityEngine;
using System.Collections.Generic;

namespace Actor
{
    public class BossAttack: MonoBehaviour
    {
        // How long the attack lasts if the player times it out
        public float durationSeconds;

        // How much damage the player must deal to cancel the attack
        public int health;

        // For special attacks, define an edgy name
        [CanBeNull] public string attackName;
        
        // For special attacks, define a capture bonus
        public int scoreBonus = 0;

        // A list of the emitters associated with this pattern
        public GameObject[] emitterPrefabs;
        
        // A list of the emitters saved for destruction later
        private List<GameObject> emitterInstances = new List<GameObject>();

        // A list of the xy coordinates for each emitter
        public int[] xCoords, yCoords;

        void Start()
        {
            SlipTimeManager slipTimeMgr = GameObject.FindObjectOfType<SlipTimeManager>();
            GameObject emitterObject;
            for (int index = 0; index < emitterPrefabs.Length; index++)
            {
                emitterObject = Instantiate(emitterPrefabs[index],
                    new Vector3(xCoords[index], yCoords[index], 0), Quaternion.identity);
                emitterObject.GetComponent<SlipTimeEmitter>().SlipTimeManager = slipTimeMgr;
                emitterObject.SetActive(true);
                emitterInstances.Add(emitterObject);
            }
        }

        public void CleanUp()
        {
            foreach (var emitter in emitterInstances)
            {
                Destroy(emitter);
            }
        }
    }
}
