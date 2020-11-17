using BulletManagement;
using JetBrains.Annotations;
using SlipTime;
using UnityEngine;
using System.Collections.Generic;

namespace Actor
{
    public class EnemyAttack: MonoBehaviour
    {
        // How long the attack lasts if the player times it out
        public float durationSeconds;

        // How much damage the player must deal to cancel the attack
        public int health;

        // For special attacks, define an edgy name
        [CanBeNull] public string attackName;
        
        // For special attacks, define a capture bonus
        public int scoreBonus = 0;

        // A list of the emitters saved for destruction later
        private readonly List<SlipTimeEmitter> emitterInstances = new List<SlipTimeEmitter>();

        private void Start()
        {
            emitterInstances.AddRange(GetComponentsInChildren<SlipTimeEmitter>());
            var slipTimeMgr = FindObjectOfType<SlipTimeManager>();
            foreach (SlipTimeEmitter emitterObject in emitterInstances)
            {
                emitterObject.SlipTimeManager = slipTimeMgr;
                emitterObject.transform.parent.gameObject.SetActive(true);
            }
        }

        public void CleanUp()
        {
            foreach (SlipTimeEmitter emitter in emitterInstances)
            {
                Destroy(emitter);
            }
        }
    }
}
