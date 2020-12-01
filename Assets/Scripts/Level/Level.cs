using System.Collections.Generic;
using System.IO;
using Actor;
using BulletManagement;
using Movement;
using SlipTime;
using UI.Score;
using UnityEngine;
using Audio;
using JetBrains.Annotations;
using UnityEngine.UI;

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
        public AudioManager audioManager;

        // Fields related to boss healthbar management
        [CanBeNull] public Text attackNameText;

        [CanBeNull] public Text timeRemaining;

        public Image healthBar;

        private Living playerLiving, bossLiving;
        private GameObject bossObject;

        private bool won, lost, waitToSpawn, bossDead = false;

        // An ordered queue of enemies to spawn
        private Queue<EnemySpawn> spawnQueue;
        private LevelEnemySpawner les = new LevelEnemySpawner();

        // Audio syncing variables
        private List<int> playSongWhenSpawnEnemyCount = new List<int>();
        private int enemySpawnedCounter = 0;

        // The next enemy to spawn in
        private EnemySpawn nextSpawn;

        // Keeps track of enemy with waitUntilDead flag
        private EnemyLiving awaitedDeath;

        // Offset between TimeSinceLevelLoad and spawn timers. Accrues when waiting for a
        // bigger enemy to die, the game is paused, etc.
        private float spawnTimeOffset;

        // Used in place of Time.TimeSinceLevelLoad to account for sliptime
        private float timeElapsed = 10f;

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
            // this.InitSun();

            // Disable boss health bar until needed
            healthBar.gameObject.SetActive(false);
            timeRemaining.gameObject.SetActive(false);
            attackNameText.gameObject.SetActive(false);

            // Start Music
            audioManager.Play();

            sliptime = GameObject.Find("SlipTimeManager").GetComponent<SlipTimeManager>();
            playerLiving = GameObject.Find("Player").GetComponent<Living>();

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
            GameObject roid3 = Resources.Load<GameObject>(Path.Combine("Enemies", "Roid_SSLs10"));
            GameObject bomber1 = Resources.Load<GameObject>(Path.Combine("Enemies", "Bomber_SNSaLa3"));
            GameObject bomber2 = Resources.Load<GameObject>(Path.Combine("Enemies", "Bomber_SNS"));
            GameObject midboss = Resources.Load<GameObject>(Path.Combine("Enemies", "MidBoss"));
            GameObject boss = Resources.Load<GameObject>(Path.Combine("Enemies", "Boss"));
            bossObject = boss;

            // Load the enemy queue

            /*
             * 00:00 - 00:10
             */
            // A wave of small ships moving top left to mid-right
            les.CurvedWave(10, 12f, saucer1, new Vector3(-6, 7, 0), EntrySide.Left);

            // A wave of small ships moving top right to mid-left
            les.CurvedWave(10, 13f, saucer2, new Vector3(6,7,0), EntrySide.Right);

            // A wave of small ships that move in from the top and leave from the sides
            les.TopWave(10, 17f, saucer3, new Vector3(1, 7, 0), EntrySide.Right, 1);
            les.TopWave(10, 17f, saucer4, new Vector3(-1, 7, 0), EntrySide.Left, 1);

            /*
             * 00:12 - 00:40
             */
            // A wave of doubled ships moving from top left to mid-right
            les.CurvedWave(10, 22f, saucer1, new Vector3(-6,7,0), EntrySide.Left, 2);
            // Meanwhile, some shooter ships enter from top right
            les.TopRow(5, 24f, saucer6, new Vector3(4,7,0), EntrySide.Right);

            // A wave of doubled ships moving from top right to mid-left
            les.CurvedWave(10, 26f, saucer2, new Vector3(6,7,0), EntrySide.Right, 2);
            // Meanwhile, some shooter ships enter from top left
            les.TopRow(5, 28f, saucer5, new Vector3(-4,7,0), EntrySide.Left);

            les.TopRow(15, 32f, saucer6, new Vector3(4, 8, 0), EntrySide.Right);
            les.TopRow(15, 36f, saucer6, new Vector3(4, 8.5f, 0), EntrySide.Right);


            // A wave of ships down the center to shoot down, while waves enter from corners with buckets of kill shots
            les.TopWave(15, 40f, saucer4, new Vector3(-1, 8, 0), EntrySide.Left, 1 );
            les.TopWave(15, 40f, saucer3, new Vector3(1, 8, 0), EntrySide.Right, 1 );
            les.CurvedWave(10, 42f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);

            les.TopWave(15, 46f, saucer4, new Vector3(-1, 8, 0), EntrySide.Left, 1 );
            les.TopWave(15, 46f, saucer3, new Vector3(1, 8, 0), EntrySide.Right, 1 );
            les.CurvedWave(10, 48f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);

            /*
             * 00:40 - 01:21
             */
            // Bomber from top to constrain followed by killshot waves
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(0, 7, 0), spawnTime = 52f});
            les.CurvedWave(10, 54f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);

            // Two bombers with a wave
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(-1, 7, 0), spawnTime = 58f});
            les.CurvedWave(10, 59f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);

            // Cue midboss build track
            spawnQueue = les.GetSpawnQueue();
            playSongWhenSpawnEnemyCount.Add(spawnQueue.Count);

            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(1, 7, 0), spawnTime = 63f});
            les.CurvedWave(10, 64f, saucer7, new Vector3(-6, 7, 0), EntrySide.Right, 1, 0.7f);

            les.TopWave(15, 70f, saucer4, new Vector3(-1, 8, 0), EntrySide.Left, 1 );
            les.TopWave(15, 70f, saucer3, new Vector3(1, 8, 0), EntrySide.Right, 1 );
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(-3, 7, 0), spawnTime = 71f});
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(3, 7, 0), spawnTime = 71f});

            les.CurvedWave(15, 75f, saucer1, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);
            les.CurvedWave(15, 75f, saucer2, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);

            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(-3, 7, 0), spawnTime = 76f});
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(3, 7, 0), spawnTime = 76f});

            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(0, 7, 0), spawnTime = 81f});

            /*
             * 01:26 - 01:35 -- build to midboss
             */
            // Bomber from top to constrain followed by killshot waves
            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(0, 7, 0), spawnTime = 86f});
            les.CurvedWave(10, 87f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);

            // Two bombers with a wave
            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(-1, 7, 0), spawnTime = 90f});
            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(1, 7, 0), spawnTime = 90f});
            les.CurvedWave(10, 91f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);

            // Three bombers with two waves
            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(0, 7, 0), spawnTime = 95f});
            les.CurvedWave(10, 96f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);
            les.CurvedWave(10, 96f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);

            // MIDBOSS
            les.Add(new EnemySpawn{enemy = midboss, spawnPosition = new Vector3(0, 4f, 0), spawnTime = 109f, waitUntilDead = true});

            /*
             * 2:10 - 2:40
             */
            // Have you played EoSD Stage 4?
            les.CurvedWave(5, 110f, roid1, new Vector3(-5, 3, 0), EntrySide.Left);

            // Trigger post-midboss level track.
            spawnQueue = les.GetSpawnQueue();
            playSongWhenSpawnEnemyCount.Add(spawnQueue.Count);

            les.CurvedWave(5, 115f, roid2, new Vector3(5, 3.5f, 0), EntrySide.Right);

            les.TopWave(5, 120f, roid3, new Vector3(-3, 6, 0), EntrySide.Left);

            les.CurvedWave(5, 125f, roid2, new Vector3(5, 4f, 0), EntrySide.Right);

            les.TopWave(5, 130f, roid3, new Vector3(3, 6, 0), EntrySide.Right);

            les.CurvedWave(5, 135f, roid1, new Vector3(-5, 4, 0), EntrySide.Left);

            les.TopWave(5, 140f, roid3, new Vector3(3, 6, 0), EntrySide.Right);

            /*
             * 2:40 - 3:00
             */
            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(3, 6, 0), spawnTime = 145});
            les.CurvedWave(5, 146, roid1, new Vector3(-5,3,0), EntrySide.Left);

            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(-3, 6, 0), spawnTime = 151});
            les.CurvedWave(5, 152, roid2, new Vector3(5,3,0), EntrySide.Right);

            les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(0, 6, 0), spawnTime = 157});
            les.CurvedWave(5, 158, roid2, new Vector3(5,3,0), EntrySide.Right);
            les.CurvedWave(5, 158, roid1, new Vector3(-5,3,0), EntrySide.Left);

            // Cue outro track
            spawnQueue = les.GetSpawnQueue();
            playSongWhenSpawnEnemyCount.Add(spawnQueue.Count);

            les.TopRow(5, 164, roid3, new Vector3(-4, 5, 0), EntrySide.Left);

            les.TopRow(5, 170, roid3, new Vector3(4, 5, 0), EntrySide.Right);

            // BOSS
            les.Add(new EnemySpawn{enemy = boss, spawnPosition = new Vector3(0, 4f, 0), spawnTime = 180f, waitUntilDead = true});

            // Sort list by time and convert to queue
            spawnQueue = les.GetSpawnQueue();
            //playSongWhenSpawnEnemyCount.Add(spawnQueue.Count);
        }

        void Update()
        {
            // Do nothing if player has won or lost
            if (lost || won) return;

            // Update time elapsed, accounting for sliptime
            timeElapsed += Time.deltaTime * sliptime.slipTimeCoefficient;

            // Handle win/lose conditions before spawning enemies
            if (!lost && playerLiving.IsDead())
            {
                lost = true;
                scoreScreenManager.Lose();
            }
            else if(bossLiving != null && !won)
            {
                if (!bossLiving.IsDead()) return;
                won = true;
                scoreScreenManager.Win();
                audioManager.PlayNextSection(true);

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

            if (waitToSpawn)
            {
                spawnTimeOffset += Time.deltaTime;
                waitToSpawn = !awaitedDeath.IsDead();
            }
            // Spawn enemies at appropriate times
            if (spawnQueue.Count > 0 && nextSpawn == null)
            {
                nextSpawn = spawnQueue.Dequeue();

                Debug.Log(enemySpawnedCounter);

                // This has to be above the if check
                enemySpawnedCounter += 1;

                // Play next song if applicable
                if (playSongWhenSpawnEnemyCount.Count > 0 && enemySpawnedCounter == playSongWhenSpawnEnemyCount[0])
                {
                    Debug.Log("Playing next section!");
                    audioManager.PlayNextSection();
                    playSongWhenSpawnEnemyCount.RemoveAt(0);
                }
            }

            if (nextSpawn == null)
            {
                bossDead = GameObject.Find("Boss").GetComponent<EnemyLiving>().IsDead();
                audioManager.PlayNextSection(true);
                return;
            }
            if (!(timeElapsed > nextSpawn.spawnTime + spawnTimeOffset)) return;
            var enemy = Instantiate(nextSpawn.enemy, nextSpawn.spawnPosition, Quaternion.identity);
            enemy.GetComponent<EnemyLiving>().SlipTimeManager = sliptime;
            enemy.GetComponent<SlipTimeMover>().SlipTimeManager = sliptime;
            foreach (var component in enemy.GetComponentsInChildren<SlipTimeEmitter>())
            {
                component.SlipTimeManager = sliptime;
            }

            // If we should wait for this enemy to die before spawning more, keep track of its Living component
            if (nextSpawn.waitUntilDead)
            {
                awaitedDeath = enemy.GetComponent<EnemyLiving>();
                waitToSpawn = true;
            }

            // If a boss-type enemy, pass in health bar fields
            var manager = enemy.GetComponent<EnemyAttackManager>();
            if(manager != null)
            {
                manager.healthBar = healthBar;
                manager.attackNameText = attackNameText;
                manager.timeRemaining = timeRemaining;
            }

            if (nextSpawn.enemy == bossObject)
            {
                bossLiving = enemy.GetComponent<EnemyLiving>();
            }
            nextSpawn = null;
        }
    }
}
