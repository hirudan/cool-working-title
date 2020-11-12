using UnityEngine;

namespace BulletManagement
{
    public class PlayerEmitter : EmitterBase
    {
        private void Start()
        {
            this.bulletPattern = this.GetComponent<BulletPattern>();
        }
        
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
                // EmitBullets();
            }
        }

        protected override void EmitBullets()
        {
            for (int id = 0; id < this.emitBulletCount; ++id)
            {
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Bullet bullet = bulletGO.GetComponent<Bullet>();
                bullet.SetData(this, this.bulletPattern, id);
                bullet.decayTime = bulletDecayTime;
            }
        }
    }
}
