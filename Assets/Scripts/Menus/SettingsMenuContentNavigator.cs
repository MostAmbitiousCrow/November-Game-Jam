using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UI;
using Void = EditorAttributes.Void;

public class SettingsMenuContentNavigator : MenuScreenContent
{
    /*
    #region Variables
    
    // [FoldoutGroup("Audio", nameof(audioMixer), 
    //     nameof(masterVolumeSlider), nameof(musicVolumeSlider), nameof(sfxVolumeSlider))]
    // [SerializeField] private Void audioFolder;
    // [SerializeField, HideProperty] private AudioMixer audioMixer;
    // [SerializeField, HideProperty] private Slider masterVolumeSlider;
    // [SerializeField, HideProperty] private Slider musicVolumeSlider;
    // [SerializeField, HideProperty] private Slider sfxVolumeSlider;
    
    [FoldoutGroup("Gameplay", nameof(invincibilityToggle), 
        nameof(dyslexicToggle), nameof(fovToggle), nameof(screenShakeToggle), nameof(rainbowModeToggle))]
    [SerializeField] private Void gameplayFolder;
    [SerializeField, HideProperty] private Toggle invincibilityToggle, dyslexicToggle, fovToggle, screenShakeToggle, 
        rainbowModeToggle;
    
    #endregion
    #region Settings
    #region Gameplay Settings
    
    private void Awake()
    {
        UpdateToggles();
    }

    private void OnEnable()
    {
        UpdateToggles();
    }

    private void UpdateToggles()
    {
        invincibilityToggle.isOn = GameSettingsManager.DoPlayerInvincibility;
        invincibilityToggle.onValueChanged.RemoveAllListeners();
        invincibilityToggle.onValueChanged.AddListener(OnPlayerInvincibilityValueChange);
        
        dyslexicToggle.isOn = GameSettingsManager.DoDyslexiaFont;
        dyslexicToggle.onValueChanged.RemoveAllListeners();
        dyslexicToggle.onValueChanged.AddListener(OnDyslexicFontValueChange);
        
        fovToggle.isOn = GameSettingsManager.DoFovSliding;
        fovToggle.onValueChanged.RemoveAllListeners();
        fovToggle.onValueChanged.AddListener(OnFovSlideValueChange);
        
        screenShakeToggle.isOn = GameSettingsManager.DoScreenShake;
        screenShakeToggle.onValueChanged.RemoveAllListeners();
        screenShakeToggle.onValueChanged.AddListener(OnScreenShakeValueChange);
        
        rainbowModeToggle.isOn = GameSettingsManager.DoRainbowMode;
        rainbowModeToggle.onValueChanged.RemoveAllListeners();
        rainbowModeToggle.onValueChanged.AddListener(OnRainbowModeValueChange);
    }

    public void OnPlayerInvincibilityValueChange(bool value)
    {
        GameSettingsManager.DoPlayerInvincibility = value;
    }

    public void OnDyslexicFontValueChange(bool value)
    {
        GameSettingsManager.DoDyslexiaFont = value;
        Debug.Log("Dyslexic font changed");
    }
    
    public void OnFovSlideValueChange(bool value)
    {
        GameSettingsManager.DoFovSliding = value;
    }

    public void OnScreenShakeValueChange(bool value)
    {
        GameSettingsManager.DoScreenShake = value;
    }
    
    public void OnRainbowModeValueChange(bool value)
    {
        GameSettingsManager.DoRainbowMode = value;
    }
    

    #endregion
    
    #region System Settings

    public void UpdateVolume(float volume)
    {
        
    }
    
    #endregion
    #endregion
    */
}
