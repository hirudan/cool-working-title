using UnityEngine;

namespace BulletManagement
{
    public class PlayerEmitter : EmitterBase
    { 
        // For playing bullet animations and audio fx
        public Animator animator;

        protected void Start()
        {
            base.Start();
            animator = transform.parent.gameObject.GetComponent<Animator>();
        }

        private void Update()
        {
            timeCounter += Time.deltaTime * bulletSpeedMultiplier;
            if (Input.GetButton("Fire1"))
            {
                // Only count if need be
                if (timeCounter >= emitFrequency)
                {
                    // As long as shoot is in the shoot state, the audio will play
                    animator.SetTrigger("Shoot");
                    EmitBullets();
                    timeCounter = 0f;
                }
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                animator.ResetTrigger("Shoot");
            }
        }
    }
}
