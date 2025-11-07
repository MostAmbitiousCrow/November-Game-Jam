using System.Collections;
using UnityEngine;

public abstract class Character_Controller_Script : MonoBehaviour
{
    public int ID;
    [SerializeField] protected bool _isConnected;
    [SerializeField] protected bool _canConnect = true;
    public bool IsConnected { get { return _isConnected; } }

    [SerializeField] protected float _detectRange = 5f;
    [SerializeField] protected LayerMask _playerMask;

    [Header("Components")]
    [SerializeField] protected Hand_Connector _handConnector;
    [SerializeField] protected Rigidbody _rb;
    public Rigidbody Rb { get { return _rb; } }

    private void FixedUpdate()
    {
        DetectPlayer();
    }

    protected void DetectPlayer()
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

                _isConnected = true;

                PlayerDetected();
            }
        }
    }

    public virtual void PlayerDetected()
    {

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

    public virtual void OnThrown()
    {
        _handConnector.UnassignConnectedHand();
        Friend_Chain_Controller.instance.RemoveFriend(ID);
        _isConnected = false;
        ID = 0;
        StartCoroutine(ChainCooldown());
    }

    protected IEnumerator ChainCooldown()
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
