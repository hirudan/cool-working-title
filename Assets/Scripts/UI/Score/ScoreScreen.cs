using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Score
{
    /// <summary>
    /// Can display the player's current score in any textbox in the application by
    /// pulling from the score manager.
    /// </summary>
    public class ScoreScreen: MonoBehaviour
    {
        /// <summary>
        /// The score text box to update and display.
        /// </summary>
        public Text scoreTextBox;

        private void Start()
        {
            scoreTextBox.text = "Final Score:\n" + PlayerStats.Score;
        }
    }
}
