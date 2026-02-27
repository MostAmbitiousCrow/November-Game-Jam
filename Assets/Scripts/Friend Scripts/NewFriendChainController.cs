using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewFriendChainController : MonoBehaviour
{
    public NewFriendController[] connectedFriends;
    [SerializeField] private float spacing = 1f; // distance between followers

    private readonly List<Vector3> _trail = new();
    //private float _distSinceLastPoint;

    public Sprite UISprite;
    [SerializeField] private Image friendUI;

    void Start()
    {
        _trail.Add(transform.position);
    }

    void FixedUpdate()
    {
        Create_trailPoint();
        MoveFriendsAlong_trail();
    }

    void Create_trailPoint()
    {
        Vector3 currentPos = transform.position;
        //_distSinceLastPoint += Vector3.Distance(currentPos, _trail[^1]);

        _trail.Add(currentPos);
        //_distSinceLastPoint = 0f;
    }

    void MoveFriendsAlong_trail()
    {
        if (connectedFriends.Length == 0) return;

        float distanceBehind = spacing;

        for (int i = 0; i < connectedFriends.Length; i++)
        {
            Vector3 targetPos = GetPointOn_trail(distanceBehind);
            connectedFriends[i].Rb.MovePosition(targetPos);
            distanceBehind += spacing;
        }
    }

    Vector3 GetPointOn_trail(float distanceBack)
    {
        // Walk backward along the _trail until we find the right point
        for (int i = _trail.Count - 1; i > 0; i--)
        {
            float segmentDist = Vector3.Distance(_trail[i], _trail[i - 1]);

            if (distanceBack <= segmentDist)
            {
                float t = distanceBack / segmentDist;
                return Vector3.Lerp(_trail[i], _trail[i - 1], t);
            }

            distanceBack -= segmentDist;
        }

        // If we run out of _trail, return the oldest point
        return _trail[0];
    }

    public void AddFriend(NewFriendController friend)
    {
        var list = new List<NewFriendController>(connectedFriends);
        list.Add(friend);
        connectedFriends = list.ToArray();
    }

    public void RemoveFriend(NewFriendController friend)
    {
        var list = new List<NewFriendController>(connectedFriends);
        list.Remove(friend);
        connectedFriends = list.ToArray();
    }
}