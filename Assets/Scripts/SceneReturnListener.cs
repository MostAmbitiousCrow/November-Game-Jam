using UnityEngine;
using UnityEngine.InputSystem;

public class SceneReturnListener : MonoBehaviour
{
    private InputAction _returnInput;
    private enum SceneType
    {
        MainMenu,
        LevelSelect,
        InGame
    }
    [Tooltip("The type of scene this listener is currently in. Will determine the correct scene to return the player to.")]
    [SerializeField] private SceneType sceneType;

    private void Awake()
    {
        _returnInput = InputSystem.actions["Cancel"];
    }

    private void Update()
    {
        if (_returnInput.WasCompletedThisFrame())
        {
            string sceneToLoad = "";

            switch (sceneType)
            {
                case SceneType.MainMenu:
                    sceneToLoad = "Main Menu";
                    break;
                case SceneType.LevelSelect:
                    sceneToLoad = "Main Menu";
                    break;
                case SceneType.InGame:
                    sceneToLoad = "Level Select";
                    break;
            }
            SceneLoader.LoadSceneRequest?.Invoke(sceneToLoad);
        }
    }
}
