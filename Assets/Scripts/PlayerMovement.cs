using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions playerInputActions;
    public Rigidbody rb;
    public float moveInput;
    public Vector3 throwInput;
    public Vector3 mousePos;
    public Vector3 moveDirection;

    public Vector3 mouseStartPoint;
    public Vector3 mouseEndPoint;

    public float speed;
    public float range;
    public float power;

    public bool arrowAppear;
    public LineRenderer arrowRenderer;
    public Material arrowMaterial;

    void Start()
    {
        arrowRenderer = GetComponent<LineRenderer>();
        arrowRenderer.material = arrowMaterial;
        arrowRenderer.startWidth = 0.5f;
        arrowRenderer.endWidth = 0.1f;
        
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
        moveInput = context.ReadValue<float>();
        if (moveInput == 1)
        {
            //gets current mouse position
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.farClipPlane * .5f;
            mouseStartPoint = Camera.main.ScreenToWorldPoint(mousePos);
            Debug.Log("Mouse pos: " + mouseStartPoint);
            arrowAppear = true;
        }

        if (moveInput == 0)
        {
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.farClipPlane * .5f;
            mouseEndPoint = Camera.main.ScreenToWorldPoint(mousePos);
            Debug.Log("Mouse pos: " + mouseEndPoint);
            //Disables the line after mouse is let go
            arrowAppear = false;
            arrowRenderer.positionCount = 0;
            
            //Finds the angle between the first and second mouse point then angles that game object in that direction
            moveDirection = mouseStartPoint - mouseEndPoint;
            float angle = Mathf.Atan2(-moveDirection.x, -moveDirection.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(-angle + 180, Vector3.forward);
            PowerCalcAndMove();
            
            //StartCoroutine(MovePlayer(power));
        }
    }
    public void OnFGrab(InputAction.CallbackContext context)
    {
        throwInput = context.ReadValue<Vector3>();
        Debug.Log(throwInput);
    }
    public void PowerCalcAndMove()
    {
        //Figures out how far the player needs to move based on the distance between the 2 mouse points
        range = Vector3.Distance(mouseStartPoint, mouseEndPoint);
        var powerX = moveDirection.x / 200;
        var powerY = moveDirection.y / 200;
        rb.AddForce(powerX, powerY, 0, ForceMode.Impulse);
        Debug.Log("X =" + powerX);
        Debug.Log("Y = " + powerY);

    }

    void Update()
    {
        if (arrowAppear)
        {
            //will draw a line to show the players headed direction if the mouse is currently held down
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.farClipPlane * .5f;
            var mousePoint = Camera.main.ScreenToWorldPoint(mousePos);
            arrowRenderer.positionCount = 2;
            arrowRenderer.SetPosition(0, transform.position);
            arrowRenderer.SetPosition(1, mousePoint);
        }
    }
   
}
