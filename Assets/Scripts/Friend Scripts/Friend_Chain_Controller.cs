using UnityEngine;
using System.Collections.Generic;

public class Friend_Chain_Controller : MonoBehaviour
{
    public static Friend_Chain_Controller instance;
    [SerializeField] List<Character_Controller_Script> _connectedHands;

    private void Awake()
    {
        instance = this;
    }

    public void AddFriend(Character_Controller_Script friend)
    {
        _connectedHands.Add(friend);
        friend.ID = _connectedHands.Count - 1;
    }

    public void RemoveFriend(int friendID)
    {
        _connectedHands.RemoveAt(friendID);
        int id = 0;
        foreach (Character_Controller_Script item in _connectedHands)
        {
            item.ID = id;
            id++;
        }
    }

    public Character_Controller_Script GetLastFriend()
    {
        return _connectedHands?[^1];
    }

    public Character_Controller_Script GetCurrentFriend()
    {
        return _connectedHands.Count > 0 ? _connectedHands[0] : null;
    }

    public bool FriendCheck()
    {
        return _connectedHands.Count > 0;
    }
}
