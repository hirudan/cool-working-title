using BulletManagement;
using UnityEngine;

namespace SlipTime
{
    public class SlipTimeManager : MonoBehaviour
    {
        private bool inSlipTime;
        
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown("Fire2") && !inSlipTime)
            {
                inSlipTime = true;
                var emitters = FindObjectsOfType<Emitter>();
                foreach (var emitter in emitters)
                {
                    emitter.bulletSpeedMultiplier = new Vector3(0.25f, 0.25f, 0.25f);
                }
            }
            else if (Input.GetButtonDown("Fire2") && inSlipTime)
            {
                inSlipTime = false;
                var emitters = FindObjectsOfType<Emitter>();
                foreach (var emitter in emitters)
                {
                    emitter.bulletSpeedMultiplier = new Vector3(1, 1, 1);
                }
            }
        }
    }
}
