using System.Linq;
using UnityEngine;

public class Menu_Manager_Main : Menu_Manager
{
#if UNITY_EDITOR

    protected override void Validation()
    {
        base.Validation();

        screenDatas = FindObjectsOfType<MenuScreenContent>();

        // Filter to only MenuScreenContent_Pause and sort by PauseMenuScreenTypes order
        var sorted = screenDatas
            .OfType<MenuScreenContent_Main>()
            .OrderBy(s => s.MainScreenTypes)
            .Cast<MenuScreenContent>()
            .ToArray();

        screenDatas = sorted;

        if (!_audioSource) _audioSource = GetComponent<AudioSource>();
    }
#endif

    protected override void ToggleScreen(int openingScreen, int closingScreen)
    {
        base.ToggleScreen(openingScreen, closingScreen);

        // Disable closing and Enable opening additional screen content
        foreach (var item in screenDatas[openingScreen].AdditionalScreenContent) { item.SetActive(true); }
        foreach (var item in screenDatas[closingScreen].AdditionalScreenContent) { item.SetActive(false); }

        MenuScreenContent OpeningScreen = screenDatas[openingScreen];
        MenuScreenContent ClosingScreen = screenDatas[closingScreen];

        // Disable closing scene and enable opening screen
        OpeningScreen.ScreenRoot.SetActive(true);
        ClosingScreen.ScreenRoot.SetActive(false);

        /* // Optionally choose to determine if the pages main button should be selected if using Keyboard or Gamepad controls
        if (// Insert check for player input here)
        {
            
        }
        */

        // Select the opening pages button
        currentScreen = openingScreen;
        if (ClosingScreen.UseExitButton) _eventSystem.SetSelectedGameObject(ClosingScreen.ExitButton.gameObject);
        else _eventSystem.SetSelectedGameObject(OpeningScreen.EnterButton.gameObject);
    }

    #region Quit Game
    public void QuitGame()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            Application.Quit(); // Quit the game
        Debug.Log("Player has quit the game.");
    }
    #endregion

    #region Game Initialisation
    public void PlayGame()
    {
        Debug.Log("Play Game Button triggered");
        ToggleInput(false);

        // Load the main game scene or trigger your scene load transition here
    }
    #endregion
}
