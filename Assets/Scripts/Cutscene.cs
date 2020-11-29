using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad >= 1f)
        {
            if (Input.anyKeyDown)
            {
                LoadMainScene();
            }
        }
    }
}
