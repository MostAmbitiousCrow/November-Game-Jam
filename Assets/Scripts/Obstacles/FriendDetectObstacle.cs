using TMPro;
using UnityEngine;

public class FriendDetectObstacle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer obstacle;
    [SerializeField] private ParticleSystem destroyEffect;
    [SerializeField] private int friendRequirement;
    [SerializeField] private TextMeshPro text;

    [SerializeField] private Collider2D detectZone;

    private void Start()
    {
        UpdateFriendCounter(0);
    }

    public void UpdateFriendCounter(int count)
    {
        text.text = $"{count}/{friendRequirement}";
        if (count >= friendRequirement) DestroyObstacle();
    }

    private void DestroyObstacle()
    {
        detectZone.enabled = false;
        obstacle.enabled = false;
        text.enabled = false;
        if (destroyEffect != null) destroyEffect.Play();
    }
}