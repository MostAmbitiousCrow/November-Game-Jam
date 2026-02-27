using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CarterGames.Assets.AudioManager;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Image friendUI;
    private Sprite friendSelected;
    [SerializeField] GameObject gamepadCursor;
    [SerializeField] List<Vector2> lastPosList;
    [SerializeField] List<Vector2> friendPosList;
    public float friendListSelect = 0;
    private InputSystem_Actions playerInputActions;
    [SerializeField] Rigidbody2D rb;
    Vector3 mousePos;
    [SerializeField] Vector3 _playerMoveDirection, _friendMovedirection;

    Vector3 mouseStartPoint;
    Vector3 mouseEndPoint;

    float speed;
    [SerializeField] float range;
    [SerializeField] float power = 3f;

    [SerializeField] bool _playerArrowActive, _friendArrowActive, _gamepadPlayerArrowActive, _gamepadFriendArrowActive;
    [SerializeField] LineRenderer _playerArrowRenderer, _friendArrowRenderer;

    [Header("Components")]
    [SerializeField] ParticleSystem _jetpackParticles;
    NewFriendChainController friendChainController;

    //public Menu_Manager_Pause pauseMenu;
    [Header("Audio")]
    [SerializeField] InspectorAudioClipPlayer _projectSound;

    private void Awake()
    {
        if (friendChainController == null) friendChainController = GetComponent<NewFriendChainController>();

        if (friendUI == null) friendUI = GameObject.Find("FriendSelectImage").GetComponent<Image>();
        friendUI.enabled = false;
    }

    void OnEnable()
    {
        playerInputActions = new();
        playerInputActions.Enable();
        playerInputActions.Player.PlayerGrab.performed += OnPGrab;
        playerInputActions.Player.PlayerGrab.canceled += OnPGrab;
        playerInputActions.Player.FriendGrab.performed += OnFGrab;
        playerInputActions.Player.FriendGrab.canceled += OnFGrab;
        playerInputActions.Player.FriendScroll.performed += OnFScroll;
        playerInputActions.Player.FriendScroll.canceled += OnFScroll;
        playerInputActions.Player.PlayerMoveGamepad.performed += OnPMoveGamepad;
        playerInputActions.Player.PlayerMoveGamepad.canceled += OnPMoveGamepad;
        playerInputActions.Player.FriendThrowGamepad.performed += OnFThrowGamepad;
        playerInputActions.Player.FriendThrowGamepad.canceled += OnFThrowGamepad;
    }
    void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Player.PlayerGrab.performed -= OnPGrab;
        playerInputActions.Player.PlayerGrab.canceled -= OnPGrab;
        playerInputActions.Player.FriendGrab.performed -= OnFGrab;
        playerInputActions.Player.FriendGrab.canceled -= OnFGrab;
        playerInputActions.Player.FriendScroll.performed -= OnFScroll;
        playerInputActions.Player.FriendScroll.canceled -= OnFScroll;
        playerInputActions.Player.PlayerMoveGamepad.performed -= OnPMoveGamepad;
        playerInputActions.Player.PlayerMoveGamepad.canceled -= OnPMoveGamepad;
        playerInputActions.Player.FriendThrowGamepad.performed -= OnFThrowGamepad;
        playerInputActions.Player.FriendThrowGamepad.canceled -= OnFThrowGamepad;
    }

    public void OnPGrab(InputAction.CallbackContext context)
    {
        //Reads whether the LMB is being pressed
        float moveInput = context.ReadValue<float>();
        if (moveInput == 1f)
        {
            //gets current mouse position
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.farClipPlane * .5f;
            mouseStartPoint = Camera.main.ScreenToWorldPoint(mousePos);
            //Debug.Log("Mouse pos: " + mouseStartPoint);
            _playerArrowActive = true;
        }

        if (moveInput == 0)
        {
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.farClipPlane * .5f;
            mouseEndPoint = Camera.main.ScreenToWorldPoint(mousePos);
            //Debug.Log("Mouse pos: " + mouseEndPoint);
            //Disables the line after mouse is let go
            _playerArrowActive = false;
            _playerArrowRenderer.positionCount = 0;
            
            //Finds the angle between the first and second mouse point then angles that game object in that direction
            _playerMoveDirection = mouseStartPoint - mouseEndPoint;
            float angle = Mathf.Atan2(-_playerMoveDirection.x, -_playerMoveDirection.y) * Mathf.Rad2Deg;
            //rb.MoveRotation(Quaternion.AngleAxis(-angle + 180, Vector3.forward)); // Rb alternative
            transform.rotation = Quaternion.AngleAxis(-angle + 180, Vector3.forward);
            PowerCalcAndMove();
            
            //StartCoroutine(MovePlayer(power));
        }
    }
    public void OnFGrab(InputAction.CallbackContext context)
    {   
        if (friendChainController.connectedFriends.Length < 1) return;

        //Friend to throw is based on element in a list
        var selectedFriend = friendChainController.connectedFriends[(int)friendListSelect];
            ;//Friend_Chain_Controller.instance.GetCurrentFriend();

        //Reads whether the LMB is being pressed
        float moveInput = context.ReadValue<float>();
        if (moveInput == 1f)
        {
            //gets current mouse position
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.farClipPlane * .5f;
            mouseStartPoint = Camera.main.ScreenToWorldPoint(mousePos);
            //Debug.Log("Mouse pos: " + mouseStartPoint);
            _friendArrowActive = true;
        }

        if (moveInput == 0)
        {
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.farClipPlane * .5f;
            mouseEndPoint = Camera.main.ScreenToWorldPoint(mousePos);
            //Debug.Log("Mouse pos: " + mouseEndPoint);
            //Disables the line after mouse is let go
            _friendArrowActive = false;
            _friendArrowRenderer.positionCount = 0;

            //Finds the angle between the first and second mouse point then angles that game object in that direction
            _friendMovedirection = mouseStartPoint - mouseEndPoint;
            float angle = Mathf.Atan2(-_playerMoveDirection.x, -_playerMoveDirection.y) * Mathf.Rad2Deg;
            //selectedFriend.Rb.MoveRotation(Quaternion.AngleAxis(-angle + 180, Vector3.forward)); // Rb alternative
            selectedFriend.transform.rotation = Quaternion.AngleAxis(-angle + 180, Vector3.forward);
            FriendPowerCalcAndMove(selectedFriend);
            if (friendListSelect != 0)
            {
                friendListSelect--;
            }

            //StartCoroutine(MovePlayer(power));
        }
    }
    public void PowerCalcAndMove()
    {
        //Figures out how far the player needs to move based on the distance between the 2 mouse points
        range = Vector3.Distance(mouseStartPoint, mouseEndPoint);
        var powerX = _playerMoveDirection.x * power;
        var powerY = _playerMoveDirection.y * power;
        rb.AddForce(new (powerX, powerY), ForceMode2D.Impulse);
        _jetpackParticles.Emit(10);

        _projectSound.Play();
    }
    public void FriendPowerCalcAndMove(NewFriendController selectedFriend)
    {
        //Figures out how far the player needs to move based on the distance between the 2 mouse points
        range = Vector3.Distance(mouseStartPoint, mouseEndPoint);
        var powerX = _friendMovedirection.x * power;
        var powerY = _friendMovedirection.y * power;
        selectedFriend.Rb.AddForce(new (powerX, powerY), ForceMode2D.Impulse);
        selectedFriend.OnThrown();

        //_projectSound.Play();
    }

    public void OnFScroll(InputAction.CallbackContext context)
    {
        //Goes through list of connected friends
        float scrollInput = context.ReadValue<float>();
        friendListSelect += scrollInput;
        
        //bounds so selected element is never under or over the list count
        if (friendListSelect < 0 && friendChainController.connectedFriends.Length > 0)
        {
            friendListSelect = friendChainController.connectedFriends.Length - 1;
        }

        if (friendListSelect < 0 && friendChainController.connectedFriends.Length == 0)
        {
            friendListSelect = 0;
        }

        if (friendListSelect >= friendChainController.connectedFriends.Length)
        {
            friendListSelect = 0;
        }
        
        
    }

    public void OnPMoveGamepad(InputAction.CallbackContext context)
    {
        var moveInput = context.ReadValue<Vector2>();
        gamepadCursor.transform.position = new Vector2(gameObject.transform.position.x + (moveInput.x * 3f), gameObject.transform.position.y + (moveInput.y * 3f));
        if (context.phase == InputActionPhase.Performed)
        {
            var lastPos = new Vector2(moveInput.x, moveInput.y);
            lastPosList.Add(lastPos);
            if (lastPosList.Count >= 4)
            {
                lastPosList.RemoveAt(0);
            }

            _gamepadPlayerArrowActive = true;
            //Debug.Log(lastPos);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            //Debug.Log(lastPos);
            float angle = Mathf.Atan2(lastPosList[0].x, lastPosList[0].y) * Mathf.Rad2Deg;
            //rb.MoveRotation(Quaternion.AngleAxis(-angle + 180, Vector3.forward)); // Rb alternative
            rb.MoveRotation(Quaternion.AngleAxis(-angle + 180, Vector3.forward)); // TODO;
            range = Vector3.Distance(gameObject.transform.position, lastPosList[0]);
            var powerX = -lastPosList[0].x * power;//_playerMoveDirection.x;
            var powerY = -lastPosList[0].y * power;//_playerMoveDirection.y;
            rb.AddForce(new(powerX, powerY), ForceMode2D.Impulse);
            _jetpackParticles.Emit(10);

            _projectSound.Play();

            _gamepadPlayerArrowActive = false;
        }
        
    }

    public void OnFThrowGamepad(InputAction.CallbackContext context)
    {   
        //Friend to throw is based on element in a list
        NewFriendController selectedFriend = friendChainController.connectedFriends[(int)friendListSelect];
        
        
        var moveInput = context.ReadValue<Vector2>();
        gamepadCursor.transform.position = new Vector2(gameObject.transform.position.x + (moveInput.x * 3f), gameObject.transform.position.y + (moveInput.y * 3f));
        if (context.phase == InputActionPhase.Performed)
        {
            var lastPos = new Vector2(moveInput.x, moveInput.y);
            friendPosList.Add(lastPos);
            if (friendPosList.Count >= 4)
            {
                friendPosList.RemoveAt(0);
            }
            _gamepadFriendArrowActive = true;
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            //Finds the angle between the first and second mouse point then angles that game object in that direction
            float angle = Mathf.Atan2(friendPosList[0].x, friendPosList[0].y) * Mathf.Rad2Deg;
            //selectedFriend.Rb.MoveRotation(Quaternion.AngleAxis(-angle + 180, Vector3.forward)); // Rb alternative
            selectedFriend.Rb.MoveRotation(Quaternion.AngleAxis(-angle + 180, Vector3.forward)); // TODO
        
            range = Vector3.Distance(gameObject.transform.position, friendPosList[0]);
            var powerX = -friendPosList[0].x * 20f;//_playerMoveDirection.x;
            var powerY = -friendPosList[0].y * 20f;//_playerMoveDirection.y;
            selectedFriend.Rb.AddForce(new (powerX, powerY), ForceMode2D.Impulse);
            selectedFriend.OnThrown();
            if (friendListSelect != 0)
            {
                friendListSelect--;
            }
            
            _gamepadFriendArrowActive = false;
        }
    }

    void Update()
    {
        //Shows friend head at bottom of screen
        if (friendChainController.connectedFriends.Length > 0)
        {
            friendSelected = friendChainController.connectedFriends[(int)friendListSelect].UISprite;
            friendUI.enabled = true;
            friendUI.sprite = friendSelected;
        }
        //Removes friend head when nothing held
        if (friendChainController.connectedFriends.Length < 1)
        {
            friendSelected = null;
            friendUI.enabled = false;
            friendUI.sprite = null;
        }
        
        if (_playerArrowActive)
        {
            //will draw a line to show the players headed direction if the mouse is currently held down
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.farClipPlane * .5f;
            var mousePoint = Camera.main.ScreenToWorldPoint(mousePos);
            _playerArrowRenderer.positionCount = 2;
            _playerArrowRenderer.SetPosition(0, mouseStartPoint);
            _playerArrowRenderer.SetPosition(1, mousePoint);
        }
        if (_friendArrowActive)
        {
            //will draw a line to show the players headed direction if the mouse is currently held down
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.farClipPlane * .5f;
            var mousePoint = Camera.main.ScreenToWorldPoint(mousePos);
            _friendArrowRenderer.positionCount = 2;
            _friendArrowRenderer.SetPosition(0, mouseStartPoint);
            _friendArrowRenderer.SetPosition(1, mousePoint);
        }

        if (_gamepadPlayerArrowActive)
        {
            _playerArrowRenderer.positionCount = 2;
            _playerArrowRenderer.SetPosition(0, gameObject.transform.position);
            _playerArrowRenderer.SetPosition(1, gamepadCursor.transform.position);
        }

        if (_gamepadFriendArrowActive)
        {
            _friendArrowRenderer.positionCount = 2;
            _friendArrowRenderer.SetPosition(0, gameObject.transform.position);
            _friendArrowRenderer.SetPosition(1, gamepadCursor.transform.position);
        }
        
        // Not Required, Pause Menu already has an input reader
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    pauseMenu.ShowPauseMenu();
        //}
    }
   
}
