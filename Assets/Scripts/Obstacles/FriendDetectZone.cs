using UnityEngine;
using UnityEngine.Events;

public class FriendDetectZone : MonoBehaviour
{
    [SerializeField] private int count;
    [SerializeField] private FriendDetectObstacle root;

    private void Awake()
    {
        if (root == null) root = GetComponentInParent<FriendDetectObstacle>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Friend"))
        {
            count++;
            root.UpdateFriendCounter(count);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Friend"))
        {
            count--;
            root.UpdateFriendCounter(count);
        }
    }
}
