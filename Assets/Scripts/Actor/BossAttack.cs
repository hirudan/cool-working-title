using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Actor
{
    public class BossAttack: MonoBehaviour
    {
        public float durationSeconds;

        public int health;

        [CanBeNull] public string name;
        
        /// <summary>
        /// The score text box to update and display.
        /// </summary>
        public Text attackNameText;

        public int scoreBonus = 0;

        // A list of the emitters associated with this pattern
        public GameObject[] emitterPrefabs;

        // A list of the xy coordinates for each emitter
        public int[] xCoords, yCoords;

        void Start()
        {
            for (int index = 0; index < emitterPrefabs.Length; index++)
            {
                Instantiate(emitterPrefabs[index],
                    new Vector3(xCoords[index], yCoords[index], 0), Quaternion.identity);
            }

            if (!string.IsNullOrEmpty(name))
            {
                attackNameText.text = name;
            }
        }

        public void CleanUp()
        {
            foreach (var emitter in emitterPrefabs)
            {
                Destroy(emitter);
            }

            attackNameText.text = "";
        }
    }
}
