using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Score
{
    /// <summary>
    /// Handles displaying and updating the player score.
    /// </summary>
    public class Score : MonoBehaviour
    {
        /// <summary>
        /// The score text box to update and display.
        /// </summary>
        public Text scoreTextBox;

        private int score;

        private void Start()
        {
            PlayerStats.Score = 0;
            score = PlayerStats.Score;
        }

        // Update is called once per frame
        private void Update()
        {
            if (PlayerStats.Score != score)
            {
                score = PlayerStats.Score;
                scoreTextBox.text = "Score\n" + score;
            }
        }
    }
}
