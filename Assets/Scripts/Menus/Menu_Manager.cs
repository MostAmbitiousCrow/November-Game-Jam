using CarterGames.Assets.AudioManager;
using EditorAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InspectorAudioClipPlayer))]
[RequireComponent(typeof(Menu_Transition_Controller))]
public abstract class Menu_Manager : MonoBehaviour // By Samuel White
{
    /*
    ========================================
    Main Menu Manager:
    Manages the collection of Screens along
    with additional sounds for buttons

    Features:
    Sounds that play upon entering/exiting a
    scene.
    Toggles for requierd menu sections.
    Sounds that will play upon selecting
    buttons
    Navigation button selection for 
    Gamepad/Keyboard controls.
    
    Depends on the Main_Menu_Transition_Controller
    for triggering any transitions you wish
    to add to your main menu. You will need
    to add the code yourself, but message
    me (mostambitiouscrow) on Discord if
    you need help!
    ========================================
    */

    #region Variables
    [Header("Components")]
    [SerializeField] protected EventSystem _eventSystem;
    [SerializeField] protected InspectorAudioClipPlayer _audioSource;
    [SerializeField] protected Canvas _canvas;

    [Header("Transition Components")]
    //[SerializeField] Settings_Menu_Manager settingsMenuManager;
    [SerializeField] protected Menu_Transition_Controller _transitionArtController;

    [Header("Menu Data")]
    [SerializeField] protected MenuScreenContent[] screenDatas;
    [SerializeField, ReadOnly] protected int currentScreen;
    [SerializeField] protected int _startScreen;

    #endregion

    private void Start()
    {
        screenDatas = null;
        SortScreens();

        Menu_Transition_Controller.OnTransitionStarted += ScreenOpened;
        Menu_Transition_Controller.OnTransitionWaiting += ToggleScreen;
        Menu_Transition_Controller.OnTransitionWaitCompleted += ScreenClosed;
        //Main_Menu_Transition_Controller.OnTransitionCompleted +=  // Something

        _eventSystem = FindFirstObjectByType<EventSystem>();

        foreach (var item in screenDatas)
            item.ScreenRoot.SetActive(false);

        screenDatas[_startScreen].ScreenRoot.SetActive(true);

        currentScreen = _startScreen;
        _eventSystem.SetSelectedGameObject(screenDatas[currentScreen].EnterButton.gameObject);
    }

    private void OnEnable()
    {
        Menu_Transition_Controller.OnTransitionStarted += ScreenOpened;
        Menu_Transition_Controller.OnTransitionWaiting += ToggleScreen;
        Menu_Transition_Controller.OnTransitionWaitCompleted += ScreenClosed;
        //Main_Menu_Transition_Controller.OnTransitionCompleted +=  // Something
    }
    private void OnDestroy()
    {
        Menu_Transition_Controller.OnTransitionStarted -= ScreenOpened;
        Menu_Transition_Controller.OnTransitionWaiting -= ToggleScreen;
        Menu_Transition_Controller.OnTransitionWaitCompleted -= ScreenClosed;
        //Main_Menu_Transition_Controller.OnTransitionCompleted -=  // Something
    }

    #region Screen Methods
    public void InvokeScreen(int type)
    {
        _transitionArtController.TriggerTransition(type, currentScreen);
        print(type + " was invoked");
    }

    protected virtual void ToggleScreen(int openingScreen, int closingScreen) 
    {
        var closingRoot = screenDatas[closingScreen].ScreenRoot;
        if (closingRoot != null) closingRoot.SetActive(false);
        var openingRoot = screenDatas[openingScreen].ScreenRoot;
        if (openingRoot != null) openingRoot.SetActive(true);
    }

    protected void ScreenOpened(int screen)
    {
        MenuScreenContent sd = screenDatas[currentScreen];
        _audioSource.Play();
        ToggleInput(false);
        sd.TriggerEvent.Invoke();
    }

    protected void ScreenClosed(int screen)
    {
        currentScreen = screen;
        MenuScreenContent sd =  screenDatas[currentScreen];
        ToggleInput(true);

        if (sd != null) _eventSystem.SetSelectedGameObject(sd.EnterButton.gameObject);
    }

    public abstract void SortScreens();
    #endregion

    #region Input Toggle
    protected void ToggleInput(bool state)
    {
        if (_eventSystem) _eventSystem.enabled = state;
        else (_eventSystem = FindFirstObjectByType<EventSystem>()).enabled = state;
    }
    #endregion

    #region Play Sounds
    // ============================= Play Sounds =============================

    /*
    public void PlaySound_UIHover() => AudioManager.PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes.Button_Hover, .5f);
    public void PlaySound_UIPress() => AudioManager.PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes.Button_Press, .5f);
    public void PlaySound_UIBack() => AudioManager.PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes.Button_Back, .5f);
    public void PlaySound_UIStartGame() => AudioManager.PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes.Button_GameStart, .5f);
    */

    #endregion

    #region OnValidation

#if UNITY_EDITOR
    private void OnValidate()
    {
        Validation();
    }

    protected virtual void Validation()
    {
        if (Application.isPlaying) return;
        //if (_canvas) _canvas.gameObject.SetActive(_showCanvas);
    }
#endif

#endregion
}