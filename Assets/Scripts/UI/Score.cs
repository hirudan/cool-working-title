using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Handles displaying and updating the player score.
    /// </summary>
    public class Score : MonoBehaviour
    {
        // TODO remove this field when we implement player scoring
        public int score;
        
        /// <summary>
        /// The score text box to update and display.
        /// </summary>
        public Text scoreTextBox;

        // Update is called once per frame
        private void Update()
        {
            score += 1;
            scoreTextBox.text = "Score\n" + score;
        }
    }
}
