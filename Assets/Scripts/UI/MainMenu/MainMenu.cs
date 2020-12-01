using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    /// <summary>
    /// Controls interactions with the main menu.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// The scene to load when the player clicks "Start".
        /// </summary>
        public string gameScene;

        /// <summary>
        /// The scene to load when the player clicks "Records".
        /// </summary>
        public string recordsScene;

        /// <summary>
        /// The scene to load when the player clicks "Options".
        /// </summary>
        public string optionsScene;

        /// <summary>
        /// The scene to load when the player clicks "Staff".
        /// </summary>
        public string staffScene;

        public GameObject mainMenuAudio;

        public void OnStartClicked()
        {
            Destroy(mainMenuAudio);
            SceneManager.LoadScene(gameScene);
        }

        public void OnRecordsClicked()
        {
            SceneManager.LoadScene(recordsScene);
        }

        public void OnOptionsClicked()
        {
            SceneManager.LoadScene(optionsScene);
        }

        public void OnStaffClicked()
        {
            SceneManager.LoadScene(staffScene);
        }

        public void OnEndClicked()
        {
            #define UNITY_WEBGL
                Application.OpenURL("https://youtu.be/ub82Xb1C8os");
            #else
                Application.Quit();
            #endif
        }
    }
}
