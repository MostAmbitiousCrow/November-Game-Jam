using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NewFriendController : MonoBehaviour
{
    public int ID;
    [SerializeField] protected bool _isConnected;
    [SerializeField] protected bool _canConnect = true;
    public bool IsConnected { get { return _isConnected; } }

    [SerializeField] protected float _detectRange = 5f;
    [SerializeField] protected LayerMask _playerMask;
    public Rigidbody2D Rb { get; private set; }

    [Space]
    [SerializeField] protected FriendRole role;
    public FriendRole Role { get { return role; } }
    public Sprite UISprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        fcc = FindFirstObjectByType<NewFriendChainController>();
    }

    private void FixedUpdate()
    {
        DetectPlayer();
    }

    protected void DetectPlayer()
    {
        if (_isConnected || !_canConnect) return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _detectRange, _playerMask);
        if (hitColliders.Length > 0)
        {
            Collider2D col = hitColliders[0];
            if (hitColliders[0].CompareTag("Player"))
            {
                print($"{col.name} Hit");
                PlayerDetected();

                _isConnected = true;
            }
        }
    }

    private NewFriendChainController fcc;
    public void PlayerDetected()
    {
        fcc.AddFriend(this);
    }

    public virtual void OnThrown()
    {
        fcc.RemoveFriend(this);
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


    public void AttatchToPlanetPoint(Transform point)
    {
        _isConnected = true;

        //TODO: Add to planet's friend list
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