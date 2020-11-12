using UnityEngine;

namespace BulletManagement
{
    public class PlayerEmitter : EmitterBase
    { 
        // Update is called once per frame
        private void Update()
        {
            timeCounter += Time.deltaTime * bulletSpeedMultiplier;
            if (Input.GetButton("Fire1"))
            {
                if (timeCounter >= emitFrequency)
                {
                    EmitBullets();
                    timeCounter = 0f;
                }
            }
        }
    }
}
