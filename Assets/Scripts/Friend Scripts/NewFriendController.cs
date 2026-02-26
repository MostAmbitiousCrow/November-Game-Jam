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

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectRange, _playerMask);
        if (hitColliders.Length > 0)
        {
            Collider col = hitColliders[0];
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }
}
