using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    void Start()
    {
        // Check if another instance of script exists
        var selfObj = FindObjectsOfType<MainMenuAudio>();
        if (selfObj.Length > 1) { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }

}
