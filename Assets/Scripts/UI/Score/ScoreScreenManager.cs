using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Score
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

        public Image winImage, loseImage, bonusImage;

        public Button restartButton, quitButton;

        // The opacities to toggle between for the win/lose screen
        private Color opacity = new Color { a = 0.0f, b = 255f, g = 255f, r = 255f };

        void Start()
        {
            Debug.Log(loseImage);
            loseImage.color = opacity;
            winImage.color = opacity;
            bonusImage.color = opacity;
            restartButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);
        }
        
        IEnumerator TransitionImage(Image img) 
        {
            for (float ft = 0f; ft <= 1; ft += 0.1f)
            {
                opacity.a = ft;
                img.color = opacity;
                yield return new WaitForSeconds(.1f);
            }
        }

        public void Win()
        {
            restartButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
            var chance = Random.Range(1, 100);
            var imgToLoad = chance > 95.0 ? bonusImage : winImage;
            StartCoroutine(TransitionImage(imgToLoad));
        }

        public void Lose()
        {
            restartButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
            StartCoroutine(TransitionImage(loseImage));
        }
        
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
