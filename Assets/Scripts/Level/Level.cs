using System.Collections.Generic;
using System.IO;
using Actor;
using BulletManagement;
using Movement;
using SlipTime;
using UI.Score;
using UnityEngine;

namespace Level
{
    /**
    * Logic associated with a Unity Scene.
    * Can handle movement of objects and instantiation of them
    * Generally you want anything that should be at the start of the game to be
    * placed in the scene first. For example the Camera which should already be in the Scene.
    */
    public class Level : MonoBehaviour
    {
        // Camera of the scene
        Camera mainCamera;

        public string mainLightName = "Main Light";
        Light mainLight;

        public ScoreScreenManager scoreScreenManager;

        private Living playerObject, bossObject;

        private bool won, lost = false;

        // An ordered queue of enemies to spawn
        private Queue<EnemySpawn> spawnQueue;
        private LevelEnemySpawner les = new LevelEnemySpawner();

        // The next enemy to spawn in
        private EnemySpawn nextSpawn;

        // Offset between TimeSinceLevelLoad and spawn timers. Accrues when waiting for a
        // bigger enemy to die, the game is paused, etc.
        private float spawnTimeOffset;

        // Used in place of Time.TimeSinceLevelLoad to account for sliptime
        private float timeElapsed = 0f;

        private SlipTimeManager sliptime;

        private void InitCamera()
        {
            // Enable the main camera for usage
            this.mainCamera.enabled = true;
            this.mainCamera.transform.position = new Vector3(0, 0, -10);

            // Look towards negate Z axis
            this.mainCamera.transform.forward = new Vector3(0, 0, 1);

            // Background color instead of default skyline
            this.mainCamera.backgroundColor = Color.black;
            this.mainCamera.clearFlags = CameraClearFlags.SolidColor;
        }

        private void InitSun()
        {
            // Grab a directional light if possible
            // otherwise create one
            foreach (Light light in FindObjectsOfType<Light>())
            {
                if (light.type == LightType.Directional && light.name == this.mainLightName) {
                    this.mainLight = light;
                    break;
                }
            }

            if (this.mainLight == null)
            {
                GameObject lightGameObject = new GameObject(this.mainLightName);
                this.mainLight = lightGameObject.AddComponent<Light>();
                this.mainLight.type = LightType.Directional;
            }

            // Face the light as the same direction as the camera
            this.mainLight.transform.position = new Vector3(0, 0, 0);
            this.mainLight.transform.forward = new Vector3(0, 0, 1);
        }

        // Start is called before the first frame update
        private void Start()
        {
            // initialize class variables
            this.mainCamera = Camera.main;

            // Initialize Camera
            this.InitCamera();
            this.InitSun();

            sliptime = GameObject.Find("SlipTimeManager").GetComponent<SlipTimeManager>();
            playerObject = GameObject.Find("Player").GetComponent<Living>();
            bossObject = GameObject.Find("Boss").GetComponent<Living>();

            // Load enemy prefabs
            GameObject saucer1 = Resources.Load<GameObject>(Path.Combine("Enemies", "SaucerDrone_SEE"));
            GameObject saucer2 = Resources.Load<GameObject>(Path.Combine("Enemies", "SaucerDrone_SWW"));
            GameObject saucer3 = Resources.Load<GameObject>(Path.Combine("Enemies", "SaucerDrone_SSE"));
            GameObject saucer4 = Resources.Load<GameObject>(Path.Combine("Enemies", "SaucerDrone_SSW"));
            GameObject saucer5 = Resources.Load<GameObject>(Path.Combine("Enemies", "SaucerDrone_SWnwLa3"));
            GameObject saucer6 = Resources.Load<GameObject>(Path.Combine("Enemies", "SaucerDrone_SEneLa3"));
            GameObject saucer7 = Resources.Load<GameObject>(Path.Combine("Enemies", "SaucerDrone_SEELa5"));
            GameObject saucer8 = Resources.Load<GameObject>(Path.Combine("Enemies", "SaucerDrone_SWWLa5"));
            GameObject roid1 = Resources.Load<GameObject>(Path.Combine("Enemies", "Roid_EELS10"));
            GameObject roid2 = Resources.Load<GameObject>(Path.Combine("Enemies", "Roid_WWLS10"));
            GameObject bomber1 = Resources.Load<GameObject>(Path.Combine("Enemies", "Bomber_SNSaLa3"));
            GameObject boss = Resources.Load<GameObject>(Path.Combine("Enemies", "Boss"));

            // Load the enemy queue

            /*
             * 00:00 - 00:10
             */

            // A wave of small ships that move in from the top and leave from the sides
            les.TopWave(10, 7f, saucer3, new Vector3(1, 7, 0), EntrySide.Right, 1);
            les.TopWave(10, 7f, saucer4, new Vector3(-1, 7, 0), EntrySide.Left, 1);

            /*
             * 00:12 - 00:28
             */
            // A wave of doubled ships moving from top left to mid-right
            les.CurvedWave(10, 12f, saucer1, new Vector3(-6,7,0), EntrySide.Left, 2);
            // Meanwhile, some shooter ships enter from top right
            les.Add(new EnemySpawn {enemy = saucer6, spawnPosition = new Vector3(4f, 7, 0), spawnTime = 14f});
            les.Add(new EnemySpawn {enemy = saucer6, spawnPosition = new Vector3(3.3f, 7, 0), spawnTime = 14.5f});
            les.Add(new EnemySpawn {enemy = saucer6, spawnPosition = new Vector3(2.6f, 7, 0), spawnTime = 15f});
            les.Add(new EnemySpawn {enemy = saucer6, spawnPosition = new Vector3(1.9f, 7, 0), spawnTime = 15.5f});
            les.Add(new EnemySpawn {enemy = saucer6, spawnPosition = new Vector3(1.2f, 7, 0), spawnTime = 16f});

            // A wave of ships down the center to shoot down, while waves enter from corners with buckets of kill shots
            les.TopWave(15, 18f, saucer4, new Vector3(-1, 8, 0), EntrySide.Left, 1 );
            les.TopWave(15, 18f, saucer3, new Vector3(1, 8, 0), EntrySide.Right, 1 );
            les.CurvedWave(10, 20f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);

            les.TopWave(15, 24f, saucer4, new Vector3(-1, 8, 0), EntrySide.Left, 1 );
            les.TopWave(15, 24f, saucer3, new Vector3(1, 8, 0), EntrySide.Right, 1 );
            les.CurvedWave(10, 26f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);

            /*
             * 00:30 - 00:45
             */
            // Bomber from top to constrain followed by killshot waves
            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(0, 7, 0), spawnTime = 30f});
            les.CurvedWave(10, 32f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);

            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(-1, 7, 0), spawnTime = 34f});
            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(1, 7, 0), spawnTime = 34f});
            les.CurvedWave(10, 35f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);

            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(0, 7, 0), spawnTime = 37f});
            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(-1, 7, 0), spawnTime = 37f});
            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(1, 7, 0), spawnTime = 37f});
            les.CurvedWave(10, 38f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);
            les.CurvedWave(10, 38f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);

            // Sort list by time and convert to queue
            spawnQueue = les.GetSpawnQueue();
        }

        void Update()
        {
            // Update time elapsed, accounting for sliptime
            timeElapsed += Time.deltaTime * sliptime.slipTimeCoefficient;
            // Handle win/lose conditions before dealing with new enemies
            if (lost || won) return;
            if (!lost && playerObject.IsDead())
            {
                lost = true;
                scoreScreenManager.Lose();
            }
            else if(!won && bossObject.IsDead())
            {
                won = true;
                scoreScreenManager.Win();

                // Delete any living enemies which, if they have emitters as children, will
                // auto clean bullets.
                // If there is a boss, calling EnemyAttack.CleanUp() and using its emitters
                // to remove the objects
                var enemyAttacks = GameObject.FindObjectsOfType<EnemyAttack>();
                var bullets = GameObject.FindObjectsOfType<BulletManagement.Bullet>();
                foreach (var attack in enemyAttacks)
                {
                    string attackName = attack.gameObject.name;
                    Debug.Log(attackName);

                    // Delete each bullet
                    foreach (var bullet in bullets)
                    {
                        if (bullet.ownerName == attackName)
                        {
                            Destroy(bullet.gameObject);
                        }
                    }
                    attack.CleanUp();
                }
            }

            // Spawn enemies at appropriate times
            if (spawnQueue.Count > 0 && nextSpawn == null)
            {
                nextSpawn = spawnQueue.Dequeue();
            }

            if (nextSpawn == null) return;
            if (!(timeElapsed > nextSpawn.spawnTime)) return;
            var enemy = Instantiate(nextSpawn.enemy, nextSpawn.spawnPosition, Quaternion.identity);
            enemy.GetComponent<EnemyLiving>().SlipTimeManager = sliptime;
            enemy.GetComponent<SlipTimeMover>().SlipTimeManager = sliptime;
            foreach (var component in enemy.GetComponentsInChildren<SlipTimeEmitter>())
            {
                component.SlipTimeManager = sliptime;
            }
            nextSpawn = null;
        }
    }
}
