using UnityEngine;
using UnityEngine.InputSystem;

public class DevProgressController : MonoBehaviour
{
    [SerializeField] private InputAction _devResetProgress;
    [SerializeField] private InputAction _devCompleteAllLevels;
    [SerializeField] private InputAction _devIncompleteAllLevels;

    private void Update()
    {
        if (_devCompleteAllLevels.WasPressedThisFrame()) GameProgress.AchieveLevels();
        if (_devResetProgress.WasPressedThisFrame()) GameProgress.ResetProgress();
        if (_devIncompleteAllLevels.WasPressedThisFrame()) GameProgress.ResetCompletedLevels();
    }

    public void Achieve()
    {
        GameProgress.AchieveLevels();
        GameProgress.ProgressUpdate?.Invoke();

    }
    public void Reset()
    {
        GameProgress.ResetProgress();
        GameProgress.ProgressUpdate?.Invoke();
    }
    public void IncompleteLevels()
    {
        GameProgress.ResetCompletedLevels();
        GameProgress.ProgressUpdate?.Invoke();
    }
}
