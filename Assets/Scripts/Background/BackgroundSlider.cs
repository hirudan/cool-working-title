using SlipTime;
using UnityEngine;

namespace Background
{
    /// <summary>
    /// Assign to a GameObject with a Sprite to
    /// generate random object that scrolls down the y-axis until time elapsed.
    /// 
    /// Similar to the bullet class except all aspects of the class
    /// is modified by the BackgroundSpawner during instantiation
    /// to make things eaiser to calculate/code.
    /// Uses z-axis to calculate speed.
    /// </summary>
    public class BackgroundSlider : MonoBehaviour, ISlipTimeAdherent
    {
        public SlipTimeManager SlipTimeManager => slipTimeManager;
        public float maxTimeAlive = 4f;

        public SlipTimeManager slipTimeManager;
        public Vector3 spawnPosition;
        public bool reuseObjectAfterExpire = false;

        float timeAlive = 0f;

        private void Start()
        {
            spawnPosition = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            // Travel speed is proportional to how far z-axis is set in object
            // So that speed is encoded in position and can be varied
            float slowTime = Time.deltaTime * this.SlipTimeManager.slipTimeCoefficient;
            float speed = -(10f - transform.position.z);
            float travel = slowTime * speed;
            timeAlive += slowTime;

            var deltaVect = new Vector3(0, travel, 0);
            transform.Translate(deltaVect, Space.World);

            if (timeAlive >= maxTimeAlive)
            {
                if (!reuseObjectAfterExpire)
                {
                    Destroy(gameObject);
                }
                else if (reuseObjectAfterExpire)
                {
                    transform.position = spawnPosition;
                    timeAlive = 0;
                }
            }
        }

        public void SetData(SlipTimeManager slipTimeManager, Vector3 localScale, float maxTimeAlive)
        {
            this.slipTimeManager = slipTimeManager;
            transform.localScale = localScale;
            this.maxTimeAlive = maxTimeAlive;
        }
    }
}
