using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.PlayerScore
{
    public class ScoreScreenManager: MonoBehaviour
    {
        /// <summary>
        /// The scene to load when the player clicks "Restart".
        /// </summary>
        public string gameScene;

        /// <summary>
        /// The scene to load when the player clicks "Main Menu"
        /// </summary>
        public string menuScene;
        

        public void OnRestartClicked()
        {
            SceneManager.LoadScene(gameScene);
        }

        public void OnMenuClicked()
        {
            SceneManager.LoadScene(menuScene);
        }
    }
}
