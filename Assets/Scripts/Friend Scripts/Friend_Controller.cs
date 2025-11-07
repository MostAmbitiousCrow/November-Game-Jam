using EditorAttributes;
using System.Collections;
using UnityEngine;

public class Friend_Controller : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float _armLength = 1f;
    [Space]
    [SerializeField] FriendRole _role;
    public FriendRole Role { get { return _role; } }
    public int ID;
    [SerializeField] bool _isConnected;
    [SerializeField] bool _canConnect = true;
    public bool IsConnected { get { return _isConnected; } }

    [SerializeField] float _detectRange = 5f;
    [SerializeField] LayerMask _playerMask;

    [Header("Components")]
    [SerializeField] Hand_Connector _handConnector;
    [SerializeField] Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }

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
        if (_isConnected || !_canConnect) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectRange, _playerMask);
        if (hitColliders.Length > 0)
        {
            Collider col = hitColliders[0];
            if (hitColliders[0].CompareTag("Player"))
            {
                print($"{col.name} Hit");
                PlayerMovement player = col.transform.GetComponent<PlayerMovement>();
                Hand_Connector playerHand = player.HandConnector;
                playerHand.AssignConnectedHand(_handConnector);

                _handConnector.ConnectHand(playerHand);
                Friend_Chain_Controller.instance.AddFriend(this);

                _isConnected = true;
            }
        }
    }

    public void AttachToFriend(Friend_Controller friend)
    {
        friend._handConnector.AssignConnectedHand(_handConnector);
    }

    public void AttatchToPlanet(Hand_Connector hand)
    {
        _isConnected = true;
        _handConnector.ConnectHand(hand);
    }

    public void OnThrown()
    {
        _handConnector.UnassignConnectedHand();
        Friend_Chain_Controller.instance.RemoveFriend(ID);
        _isConnected = false;
        ID = 0;
        StartCoroutine(ChainCooldown());
    }

    IEnumerator ChainCooldown()
    {
        print("Thrown");
        _canConnect = false;
        yield return new WaitForSeconds(2f);
        _canConnect = true;

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
