using UnityEngine;
using EditorAttributes;

[CreateAssetMenu(fileName = "Encyclopaedia Content", menuName = "ScriptableObjects/Encyclopaedia Content", order = 1)]
public class EncyclopaediaContent : ScriptableObject
{
    public Sprite friendIcon;
    public string friendName;
    [TextArea] public string friendDescription;
}
