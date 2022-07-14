using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static void LoadLevel(string levelName)
    {
        if (Application.CanStreamedLevelBeLoaded(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogWarning("LevelLoader.LoadLevel: invalid scene name");
        }
    }

    public static void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Debug.LogWarning("LevelLoader.LoadLevel: invalid scene index");
        }
    }

    public static void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public static void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int totalSceneCount = SceneManager.sceneCountInBuildSettings;
        // wraps between total count and 0
        nextSceneIndex = nextSceneIndex % totalSceneCount;
        nextSceneIndex = Mathf.Clamp(nextSceneIndex, 0, totalSceneCount);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
