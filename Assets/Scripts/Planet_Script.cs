using System.Collections.Generic;
using UnityEngine;

public class Planet_Script : MonoBehaviour
{
    [SerializeField] Transform _artTransform;
    [SerializeField] Transform _pointRootTransform;
    [SerializeField] float _rotateSpeed;

    [Header("Friends")]
    [SerializeField] List<Friend_Controller> _connectedFriends;
    [SerializeField] List<Hand_Connector> _handPoints;
    [SerializeField] int _currentPoint = 0;

    private void FixedUpdate()
    {
        RotatePlanet();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friend"))
        {
            var friend = other.GetComponent<Friend_Controller>();
            if(!friend.IsConnected) AddFriendToOrbit(friend);
        }
    }

    void AddFriendToOrbit(Friend_Controller friend)
    {
        if (_currentPoint > _handPoints.Count - 1) return;

        friend.AttatchToPlanet(_handPoints[_currentPoint]);
        _currentPoint++;
        _connectedFriends.Add(friend);
        print($"{friend.name} added to orbit!");
    }

    void RotatePlanet()
    {
        _artTransform.Rotate(_rotateSpeed * Time.fixedDeltaTime * Vector3.up);

        foreach (var point in _handPoints)
        {
            // Calculate the direction to the target
            float angle = _rotateSpeed * Time.fixedDeltaTime;

            // Rotate around the target
            point.transform.RotateAround(transform.position, Vector3.forward, angle);
        }
    }

    public void RemoveFriends()
    {
        foreach (var hand in _handPoints)
        {
            hand.UnassignConnectedHand();
        }
        _handPoints.Clear();
    }
}
