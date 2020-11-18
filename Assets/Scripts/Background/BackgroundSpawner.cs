using SlipTime;
using UnityEngine;

namespace Background
{
    /// <summary>
    /// Similar to emitter class, spawns a background object for looping.
    /// Different in that object spawned is of backgroundSlider class,
    /// which is easier to manage and all metadata including manager is managed by the
    /// Spawner. This makes Unity prefab easier to manage at the cost of slight performance.
    /// </summary>
    public class BackgroundSpawner : MonoBehaviour, ISlipTimeAdherent
    {
        public SlipTimeManager SlipTimeManager => SlipTimeManager;
        public BackgroundSlider backgroundSlider;

        // Random reflect
        public bool randomImageMirror = false;

        // Random emitter settings
        public float emitRateMin = 0f;
        public float emitRateMax = 1f;
        public float pickedEmitRate = 0f;

        // Random scaling settings
        /// <summary>
        /// Minimum scale range that emitter can emit
        /// Must be integer to preserve proper sprite scaling.
        /// </summary>
        public int minLocalScale = 1;

        /// <summary>
        /// Maximum scale range that emitter can emit
        /// Must be integer to preserve proper sprite scaling.
        /// </summary>
        public int maxLocalScale = 3;
        public Vector3 pickedLocalScale;

        // Easier just for design to delegate snapping rather than auto-calc
        public bool snapToEdge = false;

        // Counter logic
        public float timeCounter = 0f;
        public float timeAlive = 4f;

        [SerializeField]
        private SlipTimeManager slipTimeManager;

        [SerializeField]
        // Same as where the object is placed in scene
        private Vector3 spawnPosition;
        // Same rot as the object is placed in scene
        private Quaternion spawnRotation;

        public void RandSelectEmitRate()
        {
            // Pick a random emitRate Threshold
            pickedEmitRate = Random.Range(emitRateMin, emitRateMax);
        }

        public void RandEmitScale()
        {
            pickedLocalScale = Vector3.one * (float) Random.Range(minLocalScale, maxLocalScale);
        }

        private void Start()
        {
            RandSelectEmitRate();
            RandEmitScale();
        }

        // Update is called once per frame
        private void Update()
        {
            timeCounter += Time.deltaTime * slipTimeManager.slipTimeCoefficient;
            
            if (timeCounter >= pickedEmitRate)
            {
                Vector3 spawnLocation = transform.position;

                if (snapToEdge)
                {
                    // Clamp object to nearest camera side
                    Vector3 cameraPos = Camera.main.WorldToViewportPoint(spawnLocation);
                    cameraPos.x = Mathf.Clamp01(cameraPos.x);
                    spawnLocation = Camera.main.ViewportToWorldPoint(cameraPos);
                }

                BackgroundSlider go = Instantiate(backgroundSlider, spawnLocation, transform.rotation);
                go.SetData(slipTimeManager, pickedLocalScale, timeAlive);
                var sprite = go.GetComponent<SpriteRenderer>();

                // Flip spirte if possible
                if (randomImageMirror && Random.Range(0, 1) == 1)
                {
                    sprite.flipY = true;
                }

                timeCounter = 0f;
                RandSelectEmitRate();
                RandEmitScale();
            }
        }
    }
}
