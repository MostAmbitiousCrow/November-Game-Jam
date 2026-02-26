using System.Collections.Generic;
using UnityEngine;

public class NewFriendChainController : MonoBehaviour
{
    [SerializeField] private NewFriendController[] connectedFriends;
    [SerializeField] private float spacing = 1f; // distance between followers
    [SerializeField] private float trailResolution = 0.2f; // how often we record a point

    private readonly List<Vector3> trail = new();
    private float distSinceLastPoint;

    void Start()
    {
        trail.Add(transform.position);
    }

    void FixedUpdate()
    {
        CreateTrailPoint();
        MoveFriendsAlongTrail();
    }

    void CreateTrailPoint()
    {
        Vector3 currentPos = transform.position;
        distSinceLastPoint += Vector3.Distance(currentPos, trail[^1]);

        trail.Add(currentPos);
        distSinceLastPoint = 0f;
    }

    void MoveFriendsAlongTrail()
    {
        if (connectedFriends.Length == 0) return;

        float distanceBehind = spacing;

        for (int i = 0; i < connectedFriends.Length; i++)
        {
            Vector3 targetPos = GetPointOnTrail(distanceBehind);
            connectedFriends[i].Rb.MovePosition(targetPos);
            distanceBehind += spacing;
        }
    }

    Vector3 GetPointOnTrail(float distanceBack)
    {
        // Walk backward along the trail until we find the right point
        for (int i = trail.Count - 1; i > 0; i--)
        {
            float segmentDist = Vector3.Distance(trail[i], trail[i - 1]);

            if (distanceBack <= segmentDist)
            {
                float t = distanceBack / segmentDist;
                return Vector3.Lerp(trail[i], trail[i - 1], t);
            }

            distanceBack -= segmentDist;
        }

        // If we run out of trail, return the oldest point
        return trail[0];
    }

    public void AddFriend(NewFriendController friend)
    {
        var list = new List<NewFriendController>(connectedFriends);
        list.Add(friend);
        connectedFriends = list.ToArray();
    }
}