﻿using System.Collections.Generic;
using System.IO;
using Actor;
using BulletManagement;
using Movement;
using SlipTime;
using UI.Score;
using UnityEngine;
using Audio;

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

        private Living playerObject, bossObject;

        private bool won, lost = false;

        // An ordered queue of enemies to spawn
        private Queue<EnemySpawn> spawnQueue;
        private LevelEnemySpawner les = new LevelEnemySpawner();

        // Audio syncing variables
        private List<int> playSongWhenSpawnEnemyCount = new List<int>();
        private int enemySpawnedCounter = 0;


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

            // Start Music
            audioManager.Play();

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
            GameObject bomber2 = Resources.Load<GameObject>(Path.Combine("Enemies", "Bomber_SNS"));
            GameObject boss = Resources.Load<GameObject>(Path.Combine("Enemies", "Boss"));

            // Load the enemy queue

            /*
             * 00:00 - 00:10
             */
            // A wave of small ships moving top left to mid-right
            les.CurvedWave(10, 2f, saucer1, new Vector3(-6, 7, 0), EntrySide.Left);

            // A wave of small ships moving top right to mid-left
            les.CurvedWave(10, 3f, saucer2, new Vector3(6,7,0), EntrySide.Right);

            // A wave of small ships that move in from the top and leave from the sides
            les.TopWave(10, 7f, saucer3, new Vector3(1, 7, 0), EntrySide.Right, 1);
            les.TopWave(10, 7f, saucer4, new Vector3(-1, 7, 0), EntrySide.Left, 1);

            /*
             * 00:12 - 00:40
             */
            // A wave of doubled ships moving from top left to mid-right
            les.CurvedWave(10, 12f, saucer1, new Vector3(-6,7,0), EntrySide.Left, 2);
            // Meanwhile, some shooter ships enter from top right
            les.TopRow(5, 14f, saucer6, new Vector3(4,7,0), EntrySide.Right);
            
            // A wave of doubled ships moving from top right to mid-left
            les.CurvedWave(10, 16f, saucer2, new Vector3(6,7,0), EntrySide.Right, 2);
            // Meanwhile, some shooter ships enter from top left
            les.TopRow(5, 18f, saucer5, new Vector3(-4,7,0), EntrySide.Left);
            
            les.TopRow(15, 22f, saucer6, new Vector3(4, 8, 0), EntrySide.Right);
            les.TopRow(15, 26f, saucer6, new Vector3(4, 8.5f, 0), EntrySide.Right);
            

            // A wave of ships down the center to shoot down, while waves enter from corners with buckets of kill shots
            les.TopWave(15, 30f, saucer4, new Vector3(-1, 8, 0), EntrySide.Left, 1 );
            les.TopWave(15, 30f, saucer3, new Vector3(1, 8, 0), EntrySide.Right, 1 );
            les.CurvedWave(10, 32f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);
            
            les.TopWave(15, 36f, saucer4, new Vector3(-1, 8, 0), EntrySide.Left, 1 );
            les.TopWave(15, 36f, saucer3, new Vector3(1, 8, 0), EntrySide.Right, 1 );
            les.CurvedWave(10, 38f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);
            
            /*
             * 00:40 - 00:70
             */
            // Bomber from top to constrain followed by killshot waves
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(0, 7, 0), spawnTime = 42f});
            les.CurvedWave(10, 44f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);
            
            // Two bombers with a wave
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(-1, 7, 0), spawnTime = 48f});
            les.CurvedWave(10, 49f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);
            
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(1, 7, 0), spawnTime = 53f});
            les.CurvedWave(10, 54f, saucer7, new Vector3(-6, 7, 0), EntrySide.Right, 1, 0.7f);
            
            les.TopWave(15, 60f, saucer4, new Vector3(-1, 8, 0), EntrySide.Left, 1 );
            les.TopWave(15, 60f, saucer3, new Vector3(1, 8, 0), EntrySide.Right, 1 );
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(-3, 7, 0), spawnTime = 61f});
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(3, 7, 0), spawnTime = 61f});
            
            les.CurvedWave(15, 65f, saucer1, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);
            les.CurvedWave(15, 65f, saucer2, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(-3, 7, 0), spawnTime = 66f});
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(3, 7, 0), spawnTime = 66f});
            
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(-3, 7, 0), spawnTime = 71f});
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(0, 7, 0), spawnTime = 71f});
            les.Add(new EnemySpawn{enemy = bomber2, spawnPosition = new Vector3(-3, 7, 0), spawnTime = 71f});


            // // Bomber from top to constrain followed by killshot waves
            // les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(0, 7, 0), spawnTime = 56f});
            // les.CurvedWave(10, 57f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);
            //
            // // Two bombers with a wave
            // les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(-1, 7, 0), spawnTime = 60f});
            // les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(1, 7, 0), spawnTime = 60f});
            // les.CurvedWave(10, 61f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);
            //
            // // Three bombers with two waves
            // les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(0, 7, 0), spawnTime = 65f});
            // les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(-1, 7, 0), spawnTime = 65f});
            // les.Add(new EnemySpawn{enemy = bomber1, spawnPosition = new Vector3(1, 7, 0), spawnTime = 65f});
            // les.CurvedWave(10, 66f, saucer7, new Vector3(-6, 7, 0), EntrySide.Left, 1, 0.7f);
            // les.CurvedWave(10, 66f, saucer8, new Vector3(6, 7, 0), EntrySide.Right, 1, 0.7f);

            playSongWhenSpawnEnemyCount.Add(les.GetSpawnQueue().Count);
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

                Debug.Log(enemySpawnedCounter);

                // This has to be above the if check
                enemySpawnedCounter += 1;

                // Play next song if applicable
                if (enemySpawnedCounter == playSongWhenSpawnEnemyCount[0])
                {
                    Debug.Log("Playing next section!");
                    audioManager.PlayNextSection();
                    playSongWhenSpawnEnemyCount.RemoveAt(0);
                }
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
