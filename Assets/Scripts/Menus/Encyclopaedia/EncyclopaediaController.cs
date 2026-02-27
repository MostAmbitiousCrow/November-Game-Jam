using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EncyclopaediaController : MonoBehaviour
{
    [Header("Page Content")]
    [SerializeField] private int currentPage = 0;
    [SerializeField] private EncyclopaediaContent[] pages;
    [Space]
    [SerializeField] private UnityEvent onExit;

    [Header("References")]
    [SerializeField] private Image friendIcon;
    [SerializeField] private TextMeshProUGUI friendNameText;
    [SerializeField] private TextMeshProUGUI friendDescriptionText;

    [Header("Controls")]
    private bool _canNavigate = true;
    private float _inputDelayTimer;
    [SerializeField] private float inputDelay = .3f;
    private InputAction _closeMenuAction;
    private InputAction _pageNavigateAction;

    void Awake()
    {
        _pageNavigateAction = InputSystem.actions["Navigate"];
        _closeMenuAction = InputSystem.actions["Cancel"];

        // Set initial page
        UpdatePage(0);
    }

    private void OnEnable()
    {
        _closeMenuAction.performed += OnCloseMenu;
    }

    private void OnDisable()
    {
        _closeMenuAction.performed -= OnCloseMenu;
    }

    private void OnCloseMenu(InputAction.CallbackContext ctx)
    {
        onExit?.Invoke();
    }

    private void Update()
    {
        // Calculate whether or not the player can navigate the menu
        if (!_canNavigate)
        {
            _inputDelayTimer += Time.deltaTime;
            if (_inputDelayTimer >= inputDelay)
            {
                _canNavigate = true;
                _inputDelayTimer = 0;
            }
            return;
        }

        var value = _pageNavigateAction.ReadValue<Vector2>();

        if (value.x > 0.5f)
        {
            NextPage();
        }
        else if (value.x < -0.5f)
        {
            PreviousPage();
        }
    }

    public void NextPage()
    {
        if (currentPage >= pages.Length - 1) return;
        currentPage++;
        UpdatePage(currentPage);
    }

    public void PreviousPage()
    {
        if (currentPage <= 0) return;
        currentPage--;
        UpdatePage(currentPage);
    }

    private void UpdatePage(int id)
    {
        var pageContent = pages[id];
        friendIcon.sprite = pageContent.friendIcon;
        friendNameText.text = pageContent.friendName;
        if (friendDescriptionText.text != null)
            friendDescriptionText.text = pageContent.friendDescription;
    }
}
