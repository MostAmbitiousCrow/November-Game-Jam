using System;
using UnityEngine;

public static class GameProgress
{
    public static Action ProgressUpdate;

    //------------------------------
    // Level Progress
    public static void UpdateCompletedLevel(int level)
    {
        if (PlayerPrefs.GetInt($"Level{level}Complete", 0) == 0)
        {
            PlayerPrefs.SetInt($"Level{level}Complete", 1);
        }
    }

    /// <summary>
    /// Check if a level has been completed by the player
    /// </summary>
    public static bool CheckCompleteLevel(int level)
    {
        Debug.Log($"Checked level {level} as Complete");
        return PlayerPrefs.GetInt($"Level{level}Complete", 0) == 1;
    }

    public static void ResetCompletedLevels()
    {
        for (int i = 1; i <= 10; i++) PlayerPrefs.SetInt($"Level{i}Complete", 0);
        Debug.Log("All levels marked as incomplete.");
    }

    public static void AchieveLevels()
    {
        for (int i = 1; i <= 10; i++) UpdateCompletedLevel(i);
        Debug.Log("All levels marked as completed.");
    }

    //------------------------------
    // Reset All Progress
    public static void ResetProgress()
    {
        ResetCompletedLevels();
    }
}
