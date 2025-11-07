using UnityEngine;
using UnityEngine.InputSystem;
using CarterGames.Assets.AudioManager;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions playerInputActions;
    [SerializeField] Rigidbody rb;
    Vector3 throwInput;
    Vector3 mousePos;
    [SerializeField] Vector3 _playerMoveDirection, _friendMovedirection;

    Vector3 mouseStartPoint;
    Vector3 mouseEndPoint;

    float speed;
    [SerializeField] float range;
    [SerializeField] float power = 5f;

    [SerializeField] bool _playerArrowActive, _friendArrowActive;
    [SerializeField] LineRenderer _playerArrowRenderer, _friendArrowRenderer;

    [Header("Components")]
    [SerializeField] Hand_Connector _currentHandConnector;
    [SerializeField] Hand_Connector _thisHandConnector;
    public Hand_Connector HandConnector { get { return _thisHandConnector; } }
    [SerializeField] ParticleSystem _jetpackParticles;

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
    }
    void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Player.PlayerGrab.performed -= OnPGrab;
        playerInputActions.Player.PlayerGrab.canceled -= OnPGrab;
        playerInputActions.Player.FriendGrab.performed -= OnFGrab;
        playerInputActions.Player.FriendGrab.canceled -= OnFGrab;
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

        Friend_Controller selectedFriend = Friend_Chain_Controller.instance.GetCurrentFriend();

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

            //StartCoroutine(MovePlayer(power));
        }

        Debug.Log(throwInput);
    }
    public void PowerCalcAndMove()
    {
        //Figures out how far the player needs to move based on the distance between the 2 mouse points
        range = Vector3.Distance(mouseStartPoint, mouseEndPoint);
        var powerX = _playerMoveDirection.x * power;
        var powerY = _playerMoveDirection.y * power;
        rb.AddForce(powerX, powerY, 0, ForceMode.Impulse);
        _jetpackParticles.Emit(10);

        _projectSound.Play();
    }
    public void FriendPowerCalcAndMove(Friend_Controller selectedFriend)
    {
        //Figures out how far the player needs to move based on the distance between the 2 mouse points
        range = Vector3.Distance(mouseStartPoint, mouseEndPoint);
        var powerX = _friendMovedirection.x * power;
        var powerY = _friendMovedirection.y * power;
        selectedFriend.Rb.AddForce(powerX, powerY, 0, ForceMode.Impulse);
        selectedFriend.OnThrown();

        //_projectSound.Play();
    }

    void Update()
    {
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
    }
   
}
