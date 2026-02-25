using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Menu_Manager_Pause : Menu_Manager
{
    private bool _isPaused;

    private InputAction _pauseAction;

    private void Awake()
    {
        _pauseAction = InputSystem.actions["Pause"];
    }
    private void Start()
    {
        Menu_Transition_Controller.OnTransitionStarted += ScreenOpened;
        Menu_Transition_Controller.OnTransitionWaiting += ToggleScreen;
        Menu_Transition_Controller.OnTransitionWaitCompleted += ScreenClosed;
        //Main_Menu_Transition_Controller.OnTransitionCompleted +=  // Something

        foreach (var item in screenDatas)
            item.ScreenRoot.SetActive(false);

        screenDatas[_startScreen].ScreenRoot.SetActive(true);

        currentScreen = _startScreen;
        if (_eventSystem == null) _eventSystem = FindFirstObjectByType<EventSystem>();
        _eventSystem.SetSelectedGameObject(screenDatas[currentScreen].EnterButton.gameObject);

        //Filter to only MenuScreenContent_Pause and sort by PauseMenuScreenTypes order
        var sorted = screenDatas
            .OfType<MenuScreenContent_Pause>()
            .OrderBy(s => s.MainScreenTypes)
            .Cast<MenuScreenContent>()
            .ToArray();

        screenDatas = sorted;

        _canvas.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        _pauseAction.performed += OnPause;
    }

    private void OnDisable()
    {
        _pauseAction.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        _isPaused = !_isPaused;
        if (_isPaused) ShowPauseMenu();
        else ClosePauseMenu();
    }

#if UNITY_EDITOR

    protected override void Validation()
    {
        base.Validation();

        screenDatas = FindObjectsByType<MenuScreenContent>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        if (!_audioSource) _audioSource = GetComponent<AudioSource>();

        //Filter to only MenuScreenContent_Pause and sort by PauseMenuScreenTypes order
        var sorted = screenDatas
            .OfType<MenuScreenContent_Pause>()
            .OrderBy(s => s.MainScreenTypes)
            .Cast<MenuScreenContent>()
            .ToArray();

        screenDatas = sorted;
    }
#endif
    public void Resume()
    {
        ClosePauseMenu();
    }

    public void QuitToMenu()
    {
        SceneLoader.LoadSceneRequest?.Invoke("Main Menu");
    }

    public void ShowPauseMenu()
    {
        _canvas.gameObject.SetActive(true);
        _isPaused = true;
    }

    void ClosePauseMenu()
    {
        _canvas.gameObject.SetActive(false);
        _isPaused = false;
    }

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

        // Select the opening screen
        currentScreen = openingScreen;

        if (ClosingScreen.UseExitButton) _eventSystem.SetSelectedGameObject(ClosingScreen.ExitButton.gameObject);
        else _eventSystem.SetSelectedGameObject(OpeningScreen.EnterButton.gameObject);
    }

    public override void SortScreens()
    {
    }
}
