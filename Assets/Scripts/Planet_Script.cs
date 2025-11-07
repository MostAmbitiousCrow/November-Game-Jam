using System.Collections.Generic;
using UnityEngine;

public class Planet_Script : MonoBehaviour
{
    [SerializeField] Transform _artTransform;
    [SerializeField] Transform _pointRootTransform;
    [SerializeField] float _rotateSpeed;

    [Header("Friends")]
    [SerializeField] List<Character_Controller_Script> _connectedFriends;
    [SerializeField] List<Hand_Connector> _handPoints;
    [SerializeField] int _currentPoint = 0;

    [Header("Objective")]
    [SerializeField] bool _hasLetter;

    private void FixedUpdate()
    {
        RotatePlanet();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friend"))
        {
            var friend = other.GetComponent<Friend_Controller>();
            if(!friend.IsConnected) AddCharacterToOrbit(friend);
        }
        else if (other.CompareTag("Letter"))
        {
            var letter = other.GetComponent<Letter_Controller>();
            if(!letter.IsConnected)
            {
                AddCharacterToOrbit(letter);
                Objective_Manager.Instance.DeliveredLetter();
            }
        }
    }

    void AddCharacterToOrbit(Character_Controller_Script character)
    {
        if (_currentPoint > _handPoints.Count - 1) return;

        character.AttatchToPlanet(_handPoints[_currentPoint]);
        _currentPoint++;
        if(character.CompareTag("Friend"))
            _connectedFriends.Add(character);
        Objective_Manager.Instance.SavedFriend();
        print($"{character.name} added to orbit!");
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
