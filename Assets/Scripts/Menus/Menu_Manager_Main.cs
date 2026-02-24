using EditorAttributes;
using System.Linq;
using UnityEngine;

public class Menu_Manager_Main : Menu_Manager
{

    [Button("Sort Main Screens")]
    public override void SortScreens()
    {
        screenDatas = null;
        screenDatas = FindObjectsByType<MenuScreenContent>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

        // Filter to only MenuScreenContent_Pause and sort by PauseMenuScreenTypes order
        var sorted = screenDatas
            .OfType<MenuScreenContent_Main>()
            .OrderBy(s => s.MainScreenTypes)
            .Cast<MenuScreenContent>()
            .ToArray();

        screenDatas = sorted;
        print("Menu Screens Sorted");
    }

#if UNITY_EDITOR

    protected override void Validation()
    {
        base.Validation();

        //if (!_audioSource) _audioSource = GetComponent<AudioSource>();
    }
#endif

    protected override void ToggleScreen(int openingScreen, int closingScreen)
    {
        base.ToggleScreen(openingScreen, closingScreen);
    }

    #region Quit Game
    public void QuitGame()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer || _transitionArtController.IsTransitioning) return;
        
        Application.Quit(); // Quit the game
        Debug.Log("Player has quit the game.");
    }
    #endregion

    #region Game Initialisation
    public void PlayGame()
    {
        if (_transitionArtController.IsTransitioning) return;
        
        Debug.Log("Play Game Button triggered");
        ToggleInput(false);

        // Load the main game scene or trigger your scene load transition here
        _eventSystem.enabled = false;
        SceneLoader.LoadSceneRequest.Invoke("Level Select"); //TODO: Create a Main Scene Manager
    }
    #endregion
}
