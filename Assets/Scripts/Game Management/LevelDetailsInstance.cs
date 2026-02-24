using UnityEngine;

public class LevelDetailsInstance : MonoBehaviour
{
    [SerializeField] private int levelID;
    public static int LevelID;

    private void Start()
    {
        LevelID = levelID;
    }

    public static void SetLevelCompleted()
    {
        GameProgress.UpdateCompletedLevel(LevelID);
    }
}
