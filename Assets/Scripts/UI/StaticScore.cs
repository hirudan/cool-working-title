using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Can display the player's current score in any textbox in the application by
    /// pulling from the score manager.
    /// </summary>
    public class StaticScore: MonoBehaviour
    {
        /// <summary>
        /// The score text box to update and display.
        /// </summary>
        public Text scoreTextBox;

        void Start()
        {
            scoreTextBox.text = "Final Score:\n" + Score.score;
        }
    }
}
