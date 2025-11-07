using EditorAttributes;
using UnityEngine;

public class Friend_Controller : Character_Controller_Script
{
    [Header("Stats")]
    [SerializeField] protected float _armLength = 1f;
    [Space]
    [SerializeField] protected FriendRole _role;
    public FriendRole Role { get { return _role; } }

    public override void PlayerDetected()
    {
        base.PlayerDetected();
        Friend_Chain_Controller.instance.AddFriend(this);

    }

    public override void OnThrown()
    {
        base.OnThrown();
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
