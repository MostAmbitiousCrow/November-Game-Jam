using System;
using UnityEngine;

public class MenuScreenContent_Pause : MenuScreenContent
{
    [Tooltip("The type of Screen")]
    [SerializeField] private PauseMenuScreenTypes _menuScreen;
    /// <summary> The type of Screen </summary>
    public PauseMenuScreenTypes MainScreenTypes { get { return _menuScreen; } }

    protected override void Validation()
    {
        base.Validation();
        _currentMenuScreen = (int)_menuScreen;
        _screenRoot.name = new string($"--- Screen ({_menuScreen} = {CurrentMenuScreen}) ---");
    }


    // NOTE: Any additional titles you want to add, just add a value to this enum
    [Serializable]
    public enum PauseMenuScreenTypes
    {
        PauseScreen = 0, Settings = 1
    }
}
