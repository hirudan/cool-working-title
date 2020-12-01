using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    private void Start()
    {
        // Check if another instance of script exists
        var selfObj = FindObjectsOfType<MainMenuAudio>();
        if (selfObj.Length > 1) { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "StaffScene")
        {
            Destroy(gameObject);
        }
    }

}
