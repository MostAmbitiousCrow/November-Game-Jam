using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using EditorAttributes;

public abstract class MenuScreenContent : MonoBehaviour
{
    /// <summary> The type of Screen</summary>
    public int CurrentMenuScreen { get { return _currentMenuScreen; } }
    [SerializeField, ReadOnly] protected int _currentMenuScreen;

    [Header("Page contents")]
    [Tooltip("The root object of the screen")]
    [SerializeField, ReadOnly] protected GameObject _screenRoot;
    /// <summary> The root object of the screen </summary>
    public GameObject ScreenRoot { get { return _screenRoot; } }

    /// <summary> The GameObject(s) containing the contents of the Screen </summary>
    public GameObject[] AdditionalScreenContent { get { return _additionalScreenContent; } }
    [Tooltip("The GameObject(s) containing the contents of the Screen")]
    [SerializeField] protected GameObject[] _additionalScreenContent;

    /// <summary> The UI Button to be selected upon entering the Screen </summary>
    public Button EnterButton { get { return _enterButton; } }
    [Tooltip("The UI Button to be selected upon entering the Screen")]
    [SerializeField] protected Button _enterButton;

    /// <summary> Should there be a button to select upon exiting this screen </summary>
    public bool UseExitButton { get { return _useExitButton; } }
    [SerializeField] protected bool _useExitButton;
    public Button ExitButton { get { return _exitButton; } }
    [SerializeField, ShowField(nameof(_useExitButton))] protected Button _exitButton;

    [Header("Sounds")]
    [Tooltip("The sound to play upon entering this screen")]
    [SerializeField] protected AudioClip _enterSFX;
    /// <summary> The sound to play upon entering this screen </summary>
    public  AudioClip EnterSFX { get { return _enterSFX; } }

    /// <summary> The sound to play upon exiting this screen </summary>
    public AudioClip ExitSFX { get { return _exitSFX; } }
    [Tooltip("The sound to play upon exiting this screen")]
    [SerializeField] AudioClip _exitSFX;

    [Space]

    [Tooltip("An additional event that will trigger upon entering this screen")]
    [SerializeField] protected UnityEvent _triggerEvent;
    /// <summary> An additional event that will trigger upon entering this screen </summary>
    public UnityEvent TriggerEvent { get { return _triggerEvent; } }

    private void OnValidate()
    {
        _screenRoot = gameObject;
        Validation();
    }

    protected virtual void Validation() { }
}
