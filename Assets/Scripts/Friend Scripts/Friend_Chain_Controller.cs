using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Friend_Chain_Controller : MonoBehaviour
{
    public static Friend_Chain_Controller instance;
    [SerializeField] List<Friend_Controller> _connectedHands;

    private void Awake()
    {
        instance = this;
    }

    public void AddFriend(Friend_Controller friend)
    {
        _connectedHands.Add(friend);
        friend.ID = _connectedHands.Count - 1;
    }

    public void RemoveFriend(int friendID)
    {
        _connectedHands.RemoveAt(friendID);
        int id = 0;
        foreach (var item in _connectedHands)
        {
            item.ID = id;
            id++;
        }
    }

    public Friend_Controller GetLastFriend()
    {
        if (_connectedHands != null)
            return _connectedHands[^1];
        else
            return null;
    }

    public Friend_Controller GetCurrentFriend()
    {
        if (_connectedHands.Count > 0)
            return _connectedHands[0];
        else
            return null;
    }

    public bool FriendCheck()
    {
        if (_connectedHands.Count > 0)
            return true;
        else
            return false;
    }
}
