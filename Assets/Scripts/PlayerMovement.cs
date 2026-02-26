using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CarterGames.Assets.AudioManager;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine.InputSystem.LowLevel;

public class PlayerMovement : MonoBehaviour
{
    public SpriteRenderer friendUI;
    private Sprite friendSelected;
    [SerializeField] GameObject gamepadCursor;
    [SerializeField] List<Vector2> lastPosList;
    [SerializeField] List<Vector2> friendPosList;
    public float friendListSelect = 0;
    private InputSystem_Actions playerInputActions;
    [SerializeField] Rigidbody rb;
    Vector3 throwInput;
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
    [SerializeField] Hand_Connector _currentHandConnector;
    [SerializeField] Hand_Connector _thisHandConnector;
    public Hand_Connector HandConnector { get { return _thisHandConnector; } }
    [SerializeField] ParticleSystem _jetpackParticles;

    public Menu_Manager_Pause pauseMenu;
    [Header("Audio")]
    [SerializeField] InspectorAudioClipPlayer _projectSound;

    void Start()
    {
        if (_currentHandConnector) _currentHandConnector.AssignConnectedHand(_thisHandConnector);
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
        if (!Friend_Chain_Controller.instance.FriendCheck())
            return;
        
        //Friend to throw is based on element in a list
        Character_Controller_Script selectedFriend = Friend_Chain_Controller.instance._connectedHands[(int)friendListSelect];
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
        var powerX = _playerMoveDirection.x;
        var powerY = _playerMoveDirection.y;
        rb.AddForce(powerX, powerY, 0, ForceMode.Impulse);
        _jetpackParticles.Emit(10);

        _projectSound.Play();
    }
    public void FriendPowerCalcAndMove(Character_Controller_Script selectedFriend)
    {
        //Figures out how far the player needs to move based on the distance between the 2 mouse points
        range = Vector3.Distance(mouseStartPoint, mouseEndPoint);
        var powerX = _friendMovedirection.x * 2f;
        var powerY = _friendMovedirection.y * 2f;
        selectedFriend.Rb.AddForce(powerX, powerY, 0, ForceMode.Impulse);
        selectedFriend.OnThrown();

        //_projectSound.Play();
    }

    public void OnFScroll(InputAction.CallbackContext context)
    {
        //Goes through list of connected friends
        float scrollInput = context.ReadValue<float>();
        friendListSelect += scrollInput;
        
        //bounds so selected element is never under or over the list count
        if (friendListSelect < 0 && Friend_Chain_Controller.instance._connectedHands.Count > 0)
        {
            friendListSelect = Friend_Chain_Controller.instance._connectedHands.Count - 1;
        }

        if (friendListSelect < 0 && Friend_Chain_Controller.instance._connectedHands.Count == 0)
        {
            friendListSelect = 0;
        }

        if (friendListSelect >= Friend_Chain_Controller.instance._connectedHands.Count)
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
            transform.rotation = Quaternion.AngleAxis(-angle + 180, Vector3.forward);
            range = Vector3.Distance(gameObject.transform.position, lastPosList[0]);
            var powerX = -lastPosList[0].x * 10f;//_playerMoveDirection.x;
            var powerY = -lastPosList[0].y * 10f;//_playerMoveDirection.y;
            rb.AddForce(powerX, powerY, 0, ForceMode.Impulse);
            _jetpackParticles.Emit(10);

            _projectSound.Play();

            _gamepadPlayerArrowActive = false;
        }
        
    }

    public void OnFThrowGamepad(InputAction.CallbackContext context)
    {
        if (!Friend_Chain_Controller.instance.FriendCheck())
            return;
        
        //Friend to throw is based on element in a list
        Character_Controller_Script selectedFriend = Friend_Chain_Controller.instance._connectedHands[(int)friendListSelect];
        
        
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
            selectedFriend.transform.rotation = Quaternion.AngleAxis(-angle + 180, Vector3.forward);
        
            range = Vector3.Distance(gameObject.transform.position, friendPosList[0]);
            var powerX = -friendPosList[0].x * 20f;//_playerMoveDirection.x;
            var powerY = -friendPosList[0].y * 20f;//_playerMoveDirection.y;
            selectedFriend.Rb.AddForce(powerX, powerY, 0, ForceMode.Impulse);
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
        if (Friend_Chain_Controller.instance._connectedHands.Count > 0)
        {
            friendSelected = Friend_Chain_Controller.instance._connectedHands[(int)friendListSelect].UISprite; ;
            friendUI.sprite = friendSelected;
        }
        //Removes friend head when nothing held
        if (Friend_Chain_Controller.instance._connectedHands.Count <= 0)
        {
            friendSelected = null;
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
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.ShowPauseMenu();
        }
    }
   
}
