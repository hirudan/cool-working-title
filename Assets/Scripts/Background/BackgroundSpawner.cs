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
        public float emitRateMax = 1f;
        public float emitRateMin = 0f;

        public float minLocalScale = 0f;
        public float maxLocalScale = 1f;

        // Easier just for design to delegate snapping rather than auto-calc
        public bool snapToEdge = false;

        public float timeCounter = 0f;
        public float timeAlive = 4f;

        [SerializeField]
        private SlipTimeManager slipTimeManager;

        [SerializeField]
        // Same as where the object is placed in scene
        private Vector3 spawnPosition;
        // Same rot as the object is placed in scene
        private Quaternion spawnRotation;

        // Update is called once per frame
        private void Update()
        {
            timeCounter += Time.deltaTime * slipTimeManager.slipTimeCoefficient;
            
            if (timeCounter >= emitRateMax)
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
                go.SetData(slipTimeManager, transform.localScale, timeAlive);
                timeCounter = 0f;
            }
        }
    }
}
