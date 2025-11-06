using System;
using UnityEngine;

public class MenuScreenContent_Main : MenuScreenContent
{
    [Tooltip("The type of Screen")]
    [SerializeField] private MainMenuScreenTypes _menuScreen;
    /// <summary> The type of Screen</summary>
    public MainMenuScreenTypes MainScreenTypes { get { return _menuScreen; } }

    //public int[] MenuScreen { get { return (int[])Enum.GetValues(typeof(MainMenuScreenTypes)); } }
    //public int MenuScreen { get { return (int)_menuScreen; } }

    protected override void Validation()
    {
        base.Validation();
        _currentMenuScreen = (int)_menuScreen;
        _screenRoot.name = new string($"--- Screen ({_menuScreen} = {(int)_menuScreen}) ---");
    }


    // NOTE: Any additional titles you want to add, just add a value to this enum
    [Serializable]
    public enum MainMenuScreenTypes
    {
        StartScreen = 0, TitleScreen = 1, SettingsScreen = 2, CreditsScreen = 3
    }
}
