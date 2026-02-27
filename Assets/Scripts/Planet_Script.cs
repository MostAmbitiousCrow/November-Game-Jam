using System.Collections.Generic;
using UnityEngine;

public class Planet_Script : MonoBehaviour
{
    [SerializeField] Transform _artTransform;
    [SerializeField] Transform _pointRootTransform;
    [SerializeField] float _rotateSpeed;

    [Header("Friends")]
    [SerializeField] List<NewFriendController> connectedFriends;
    [SerializeField] List<Transform> hangPoints;
    [SerializeField] int _currentPoint = 0;

    [Header("Objective")]
    [SerializeField] bool _hasLetter;

    private void FixedUpdate()
    {
        RotatePlanet();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Friend"))
        {
            var friend = other.GetComponent<NewFriendController>();
            if(!friend.IsConnected) AddCharacterToOrbit(friend);
        }
        else if (other.CompareTag("Letter"))
        {
            var letter = other.GetComponent<NewFriendController>();
            if(!letter.IsConnected)
            {
                AddCharacterToOrbit(letter);
                Objective_Manager.Instance.DeliveredLetter();
            }
        }
    }

    void AddCharacterToOrbit(NewFriendController character)
    {
        if (_currentPoint > hangPoints.Count - 1) return;

        character.AttatchToPlanetPoint(hangPoints[_currentPoint]);
        _currentPoint++;
        connectedFriends.Add(character);

        if (character.CompareTag("Friend"))
        {
            Objective_Manager.Instance.SavedFriend();
        }
        else if (character.CompareTag("Letter"))
        {
            _hasLetter = true;
        }
        print($"{character.name} added to orbit!");
    }

    void RotatePlanet()
    {
        _artTransform.Rotate(_rotateSpeed * Time.fixedDeltaTime * Vector3.up);

        foreach (var point in hangPoints)
        {
            // Calculate the direction to the target
            float angle = _rotateSpeed * Time.fixedDeltaTime;

            // Rotate around the target
            point.transform.RotateAround(transform.position, Vector3.forward, angle);
        }
    }

    //public void RemoveFriends()
    //{
    //    foreach (var point in hangPoints)
    //    {
    //        point.UnassignConnectedHand();
    //    }
    //    hangPoints.Clear();
    //}
}
