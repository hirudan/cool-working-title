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
        private List<EnemySpawn> spawnList = new List<EnemySpawn>();
        private Queue<EnemySpawn> spawnQueue;
        

        // The next enemy to spawn in
        private EnemySpawn nextSpawn;

        // Offset between TimeSinceLevelLoad and spawn timers. Accrues when waiting for a 
        // bigger enemy to die, the game is paused, etc.
        private float spawnTimeOffset;

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
            GameObject roid1 = Resources.Load<GameObject>(Path.Combine("Enemies", "Roid_EELS10"));
            GameObject roid2 = Resources.Load<GameObject>(Path.Combine("Enemies", "Roid_WWLS10"));
            GameObject bomber1 = Resources.Load<GameObject>(Path.Combine("Enemies", "Bomber_SNSaLa3"));

            // Load the enemy queue

            /* 
             * 00:00 - 00:08
             */
            
            // A wave of small ships moving top left to mid-right
            spawnList.Add(new EnemySpawn{spawnTime = 2.0f, enemy = saucer1, spawnPosition = new Vector3(-6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 2.2f, enemy = saucer1, spawnPosition = new Vector3(-6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 2.4f, enemy = saucer1, spawnPosition = new Vector3(-6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 2.6f, enemy = saucer1, spawnPosition = new Vector3(-6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 2.8f, enemy = saucer1, spawnPosition = new Vector3(-6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 3.0f, enemy = saucer1, spawnPosition = new Vector3(-6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 3.2f, enemy = saucer1, spawnPosition = new Vector3(-6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 3.4f, enemy = saucer1, spawnPosition = new Vector3(-6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 3.6f, enemy = saucer1, spawnPosition = new Vector3(-6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 3.8f, enemy = saucer1, spawnPosition = new Vector3(-6, 7, 0)});
            
            // A wave of small ships moving top right to mid-left
            spawnList.Add(new EnemySpawn{spawnTime = 3.0f, enemy = saucer2, spawnPosition = new Vector3(6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 3.2f, enemy = saucer2, spawnPosition = new Vector3(6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 3.4f, enemy = saucer2, spawnPosition = new Vector3(6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 3.6f, enemy = saucer2, spawnPosition = new Vector3(6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 3.8f, enemy = saucer2, spawnPosition = new Vector3(6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 4.0f, enemy = saucer2, spawnPosition = new Vector3(6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 4.2f, enemy = saucer2, spawnPosition = new Vector3(6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 4.4f, enemy = saucer2, spawnPosition = new Vector3(6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 4.6f, enemy = saucer2, spawnPosition = new Vector3(6, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 4.8f, enemy = saucer2, spawnPosition = new Vector3(6, 7, 0)});
            
            // A wave of small ships that move in from the top and leave from the sides
            spawnList.Add(new EnemySpawn{spawnTime = 6.0f, enemy = saucer3, spawnPosition = new Vector3(1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 6.2f, enemy = saucer3, spawnPosition = new Vector3(1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 6.4f, enemy = saucer3, spawnPosition = new Vector3(1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 6.6f, enemy = saucer3, spawnPosition = new Vector3(1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 6.8f, enemy = saucer3, spawnPosition = new Vector3(1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 7.0f, enemy = saucer3, spawnPosition = new Vector3(1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 7.2f, enemy = saucer3, spawnPosition = new Vector3(1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 7.4f, enemy = saucer3, spawnPosition = new Vector3(1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 7.6f, enemy = saucer3, spawnPosition = new Vector3(1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 7.8f, enemy = saucer3, spawnPosition = new Vector3(1, 7, 0)});
            
            spawnList.Add(new EnemySpawn{spawnTime = 6.0f, enemy = saucer4, spawnPosition = new Vector3(-1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 6.2f, enemy = saucer4, spawnPosition = new Vector3(-1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 6.4f, enemy = saucer4, spawnPosition = new Vector3(-1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 6.6f, enemy = saucer4, spawnPosition = new Vector3(-1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 6.8f, enemy = saucer4, spawnPosition = new Vector3(-1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 7.0f, enemy = saucer4, spawnPosition = new Vector3(-1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 7.2f, enemy = saucer4, spawnPosition = new Vector3(-1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 7.4f, enemy = saucer4, spawnPosition = new Vector3(-1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 7.6f, enemy = saucer4, spawnPosition = new Vector3(-1, 7, 0)});
            spawnList.Add(new EnemySpawn{spawnTime = 7.8f, enemy = saucer4, spawnPosition = new Vector3(-1, 7, 0)});
            
            // Sort list by time and convert to queue
            spawnList.Sort((x, y) => x.spawnTime.CompareTo(y.spawnTime));
            spawnQueue = new Queue<EnemySpawn>(spawnList);

        }

        void Update()
        {
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
            if (!(Time.timeSinceLevelLoad > nextSpawn.spawnTime)) return;
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
