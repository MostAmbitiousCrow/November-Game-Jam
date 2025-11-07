using CarterGames.Assets.AudioManager;
using UnityEngine;
using UnityEngine.XR;

public class Hand_Connector : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody LeftHandRb { get { return _leftHandRb; } }
    public Rigidbody RightHandRb { get { return _rightHandRb; } }
    [SerializeField] Rigidbody _leftHandRb, _rightHandRb;

    [SerializeField] Transform _leftHandJoint, _rightHandJoint;

    [Header("Connections")]
    // These represent the current Hands connected to this Friend
    [SerializeField] Hand_Connector _leftConnection;
    [SerializeField] Hand_Connector _rightConnection;

    public Rigidbody ConnectedRightHandRb { get { return _connectedRightHandRb; } }
    [Space]

    [SerializeField] Rigidbody _connectedRightHandRb, _connectedLeftHandRB;

    [Header("Stats")]
    [SerializeField] float _connectDistance = .05f;
    [SerializeField] float _speed = 10f;

    [Header("Audio")]
    [SerializeField] InspectorAudioClipPlayer _connectSound;

    private void Start()
    {
        if (_leftConnection) _leftConnection.AssignConnectedHand(this);
    }

    private void FixedUpdate()
    {
        // If a connected Right Hand exists, move the Left Hand Joint to follow the Connected Right Hand
        if (_rightConnection)
        {
            // Move this Friends Left Hand towards the Connected Friends Right Hand
            MoveJoint(_leftHandRb, _rightConnection._rightHandRb);
        }
    }

    void MoveJoint(Rigidbody rb, Rigidbody targetRb)
    {
        Vector3 targetPosition = targetRb.position;

        // Follow Target
        Vector3 newPosition = Vector3.Lerp(rb.position, targetPosition, _speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        // Follow rotation
        Quaternion targetRotation = targetRb.rotation;
        Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, _speed * Time.fixedDeltaTime);
        rb.MoveRotation(newRotation);
    }

    /// <summary>
    /// Function for whenver a Hand is disconnected to this Friends Right Hand
    /// </summary>
    public void DisconnectHand()
    {
        _rightConnection = null;

        _connectedRightHandRb = null;
    }

    /// <summary>
    /// Function for whenever a Hand is connected to this Friends Right Hand
    /// </summary>
    public void ConnectHand(Hand_Connector hand)
    {
        _rightConnection = hand;

        _connectedRightHandRb = hand._rightHandRb;

        _connectSound.Play();
    }

    /// <summary>
    /// Function to assign another Friends Right Hand to this Friend to follow
    /// </summary>
    public void AssignConnectedHand(Hand_Connector hand)
    {
        _leftConnection = hand;
        _connectedLeftHandRB = hand._leftHandRb;
        //_connectedLeftHandRB = hand.LeftHandRb;
    }

    /// <summary>
    /// 
    /// </summary>
    public void UnassignConnectedHand()
    {
        _rightConnection = null;
        _connectedRightHandRb = null;
        //_connectedLeftHandRB = null;
    }

}

public enum HandDisconnection
{
    Both, Left, Right
}
