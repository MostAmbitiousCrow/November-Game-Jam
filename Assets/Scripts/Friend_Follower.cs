using UnityEngine;

public class Friend_Follower : MonoBehaviour
{
    [SerializeField] Vector2 _cursorPos;
    [SerializeField] Hand_Connector _currentHandConnector;
    [SerializeField] Hand_Connector _thisHandConnector;
    public Hand_Connector HandConnector { get { return _thisHandConnector; } }
    [SerializeField] Transform _cursor;

    [SerializeField] Friend_Chain_Controller _chainController;

    [Space]
    [SerializeField] Rigidbody rb;

    private void Start()
    {
        if(_currentHandConnector) _currentHandConnector.AssignConnectedHand(_thisHandConnector);
    }
    void FixedUpdate()
    {
        CursorFolllow();

        if (Input.GetMouseButtonDown(1))
        {
            DisconnectFriend();
        }
    }

    void CursorFolllow()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        _cursorPos = Camera.main.ScreenToWorldPoint(mousePos);

        rb.MovePosition(_cursorPos);
        if (_currentHandConnector)
        {
            _currentHandConnector.RightHandRb.MovePosition(_cursorPos);
        }
    }

    void DisconnectFriend()
    {
        if(_currentHandConnector) _currentHandConnector.UnassignConnectedHand();
    }
}
