using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class LevelSelectItem : MonoBehaviour
{
    public int ID;
    public string LevelSceneName => levelSceneName;
    [SerializeField] private string levelSceneName;

    private Animator _animator;

    private readonly UnityEvent _selectEvent = new();

    private const string ACTIVATED_STRING = "Activated";
    private const string SELECTED_STRING = "Selected";
    private const string DESELECTED_STRING = "Deselected";

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        // Auto set ID
        ID = transform.GetSiblingIndex();

        var manager = FindFirstObjectByType<LevelSelectManager>();
        _selectEvent.AddListener(() => manager.SelectLevel(ID));
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
