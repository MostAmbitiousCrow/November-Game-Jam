using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSelectManager : MonoBehaviour
{

    [Header("Navigation")]
    [SerializeField] private bool navigationEnabled = true;
    [SerializeField] private int currentSelectedItem;
    [SerializeField] private LevelSelectItem[] levelSelectItems;

    [SerializeField] private int _itemCount;
    [SerializeField] private float inputDelay = .5f;
    private float _inputDelayTimer;
    [SerializeField] private bool _canNavigate = true;
    private bool _isLoadingLevel;

    private InputAction _navigateAction;
    private InputAction _enterInput;
    private InputAction _returnInput;

    [Header("Camera")]
    [SerializeField] private LevelSelectCamera levelSelectCamera;

    private void Start()
    {
        _navigateAction = InputSystem.actions["Navigate"];
        _enterInput = InputSystem.actions["Submit"];
        _returnInput = InputSystem.actions["Cancel"];

        levelSelectItems = GetComponentsInChildren<LevelSelectItem>();

        _itemCount = levelSelectItems.Length;

        if (levelSelectCamera == null) levelSelectCamera = FindFirstObjectByType<LevelSelectCamera>();
    }

    private void Update()
    {
        if (_returnInput.WasCompletedThisFrame() && !_isLoadingLevel)
        {
            SceneLoader.LoadSceneRequest?.Invoke("Main Menu");
        }
        if (!navigationEnabled) return;

        if (_enterInput.WasPerformedThisFrame())
        {
            InitialiseLevel(currentSelectedItem);
            return;
        }

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

        // Input Navigation

        var inputValue = _navigateAction.ReadValue<Vector2>();
        if (inputValue.x > .1f)
        {
            SelectLevel(currentSelectedItem+1);
            return;
        }
        if (inputValue.x < -.1f)
        {
            SelectLevel(currentSelectedItem-1);
            return;
        }
    }

    /// <summary> Select a level and automatically navigate to the level in the scene. Can only be called if the user is able to navigate.</summary>
    public void SelectLevel(int id)
    {
        var item = GetItem(id);

        NavigateToItem(item);
    }

    public void InitialiseLevel(int id)
    {
        // Requires you to have navigated to the level first!
        if (currentSelectedItem != id) return;
        navigationEnabled = false;

        //print($"Level {GetItem(id).name} selected!");

        // TODO: Load the level and transition to it
        SceneLoader.LoadSceneRequest?.Invoke(GetItem(id).LevelSceneName);
        if (!SceneLoader.IsLoadingScene) return;
        
        _isLoadingLevel = true;
    }

    private void NavigateToItem(LevelSelectItem item)
    {
        levelSelectItems[currentSelectedItem].Deselected(); //Deselect last item

        // Set new item and select it
        currentSelectedItem = item.ID;
        item.Selected();

        levelSelectCamera.AssignTarget(item.transform);

        _canNavigate = false;

        print($"Navigated to New ID: {currentSelectedItem}");
    }

    private LevelSelectItem GetItem(int id)
    {
        if (id < 0) id = _itemCount-1; // Return to the end of the array if negative
        else if (id > _itemCount-1) id = 0; // Return to the start of the array if over the limit

        print("Getting item with ID: " + id);
        return levelSelectItems[id];
    }
}
