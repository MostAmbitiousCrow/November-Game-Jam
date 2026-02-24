using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    
    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        ActivateFont();
    }

    private void ActivateFont()
    {
        //if (!textMesh || !GameSettingsManager.Instance) return;
        //textMesh.font = GameSettingsManager.DoDyslexiaFont? 
        //    GameSettingsManager.Instance.dyslexicFont : GameSettingsManager.Instance.pixelFont;

        //// Set text colour as the global highlight colour
        //if (GameColoursManager.CurrentColours == null) return;
        //textMesh.color = GameColoursManager.CurrentColours.MaterialColours[0].HighlightColour;
    }
}
