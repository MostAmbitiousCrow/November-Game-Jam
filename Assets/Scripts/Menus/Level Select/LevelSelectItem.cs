using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class LevelSelectItem : MonoBehaviour
{
    [SerializeField] private bool isComplete;
    [SerializeField] private bool isLocked; //TODO: Implement locked levels feature
    public int ID;
    public string LevelSceneName => levelSceneName;
    [SerializeField] private string levelSceneName;

    [SerializeField] private Sprite completedSprite, incompleteSprite;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private readonly UnityEvent _selectEvent = new();

    private const string ACTIVATED_STRING = "Activated";
    private const string SELECTED_STRING = "Selected";
    private const string DESELECTED_STRING = "Deselected";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Auto set ID
        ID = transform.GetSiblingIndex();

        var manager = FindFirstObjectByType<LevelSelectManager>();
        _selectEvent.AddListener(() => manager.SelectLevel(ID));

        UpdateState();
    }

    private void UpdateState()
    {
        isComplete = GameProgress.CheckCompleteLevel(ID);
        _spriteRenderer.sprite = isComplete ? completedSprite : incompleteSprite;
    }

    private void OnEnable()
    {
        GameProgress.ProgressUpdate += UpdateState;
    }
    private void OnDisable()
    {
        GameProgress.ProgressUpdate -= UpdateState;
    }

    public void Activate()
    {
        _animator.SetTrigger(ACTIVATED_STRING);
    }

    public void Selected()
    {
        _animator.SetTrigger(SELECTED_STRING);
    }

    public void Deselected()
    {
        _animator.SetTrigger(DESELECTED_STRING);
    }

    private void OnMouseDown()
    {
        // Check if the player clicked on this item
        _selectEvent.Invoke();
    }

    private void OnValidate()
    {
        name = $"Level Item ({levelSceneName})";
    }
}