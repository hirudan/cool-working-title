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

        // A list of the emitters saved for destruction later
        private List<SlipTimeEmitter> emitterInstances = new List<SlipTimeEmitter>();

        void Start()
        {
            emitterInstances.AddRange(GetComponentsInChildren<SlipTimeEmitter>());
            SlipTimeManager slipTimeMgr = FindObjectOfType<SlipTimeManager>();
            for (int index = 0; index < emitterInstances.Count; index++)
            {
                var emitterObject = emitterInstances[index];
                emitterObject.SlipTimeManager = slipTimeMgr;
                emitterObject.transform.parent.gameObject.SetActive(true);
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
