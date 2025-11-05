using EditorAttributes;
using UnityEngine;

public class Friend_Controller : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float _armLength = 1f;
    [Space]
    [SerializeField] FriendRole _role;
    [SerializeField, ReadOnly] int _ID;
    public int ID { get { return _ID; } }
    [SerializeField] bool _isConnected;
    public bool IsConnected { get { return _isConnected; } }

    [Header("")]
    [SerializeField] float _detectRange = 5f;
    [SerializeField] LayerMask _playerMask;

    [Header("Components")]
    [SerializeField] Hand_Connector _handConnector;

    //private void Start()
    //{
    //    InvokeRepeating(nameof(DetectPlayer), 0f, .1f);
    //}

    private void FixedUpdate()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        if (_isConnected) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectRange, _playerMask);
        if (hitColliders.Length > 0)
        {
            Collider col = hitColliders[0];
            if (hitColliders[0].CompareTag("Player"))
            {
                print($"{col.name} Hit");
                Friend_Follower friendFollower = col.transform.GetComponent<Friend_Follower>();
                Hand_Connector playerHand = friendFollower.HandConnector;
                playerHand.AssignConnectedHand(_handConnector);

                _handConnector.ConnectHand(playerHand);

                _isConnected = true;
            }
        }
    }

    public void AttachToFriend(Friend_Controller friend)
    {
        friend._handConnector.AssignConnectedHand(_handConnector);
    }

    public void TriggerThrow()
    {
        _isConnected = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}

public enum FriendRole
{
    NoRole, Farmer, Miner, Lumberjack
}
